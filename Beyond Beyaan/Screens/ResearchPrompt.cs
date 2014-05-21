using System;
using System.Collections.Generic;
using Beyond_Beyaan.Data_Managers;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Screens
{
	public class ResearchPrompt : WindowInterface
	{
		public Action Completed;

		private BBSprite _techIcon;

		private List<Technology> _discoveredTechs;
		private List<TechField> _fieldsNeedingNewTopics;
		private Dictionary<TechField, List<Technology>> _availableTopics;
		private TechField _currentTechField;

		private BBStretchableImage _techDescriptionBackground;
		private BBStretchableImage _availableTechsToResearchBackground;
		private BBInvisibleStretchButton[] _availableTechsToResearchButtons;
		private BBLabel[] _researchCosts;
		private BBScrollBar _scrollBar;
		private int _maxVisible;

		private BBLabel _instructionLabel;
		private BBTextBox _techDescription;

		private Empire _currentEmpire;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!this.Initialize(gameMain.ScreenWidth / 2 - 230, gameMain.ScreenHeight / 2 - 180, 460, 360, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_discoveredTechs = new List<Technology>();
			_fieldsNeedingNewTopics = new List<TechField>();
			_availableTopics = new Dictionary<TechField, List<Technology>>();

			_techDescriptionBackground = new BBStretchableImage();
			_availableTechsToResearchBackground = new BBStretchableImage();
			_instructionLabel = new BBLabel();
			_techDescription = new BBTextBox();
			_scrollBar = new BBScrollBar();
			_availableTechsToResearchButtons = new BBInvisibleStretchButton[4];
			_researchCosts = new BBLabel[4];

			if (!_techDescriptionBackground.Initialize(_xPos + 20, _yPos + 20, 420, 170, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_availableTechsToResearchBackground.Initialize(_xPos + 20, _yPos + 220, 420, 120, StretchableImageType.ThinBorderBG, gameMain.Random, out reason))
			{
				return false;
			}
			if (!_instructionLabel.Initialize(_xPos + 20, _yPos + 195, "Please select an item to research", System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_techDescription.Initialize(_xPos + 165, _yPos + 33, 265, 150, true, true, "TechDescriptionTextBox", gameMain.Random, out reason))
			{
				return false;
			}
			if (!_scrollBar.Initialize(_xPos + 415, _yPos + 230, 100, 4, 4, false, false, gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < _availableTechsToResearchButtons.Length; i++)
			{
				_availableTechsToResearchButtons[i] = new BBInvisibleStretchButton();
				_researchCosts[i] = new BBLabel();
				if (!_availableTechsToResearchButtons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, _xPos + 30, _yPos + 230 + (i * 25), 385, 25, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_availableTechsToResearchButtons[i].SetToolTip("ItemToResearch" + i + "ToolTip", string.Empty, gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_researchCosts[i].Initialize(_xPos + 405, _yPos + 232 + (i * 25), string.Empty, System.Drawing.Color.White, out reason))
				{
					return false;
				}
				_researchCosts[i].SetAlignment(true);
			}

			return true;
		}

		public override void Draw()
		{
			base.Draw();
			_techDescriptionBackground.Draw();
			_availableTechsToResearchBackground.Draw();
			_techIcon.Draw(_xPos + 35, _yPos + 38);
			_techDescription.Draw();
			_instructionLabel.Draw();
			for (int i = 0; i < _maxVisible; i++)
			{
				_availableTechsToResearchButtons[i].Draw();
				_researchCosts[i].Draw();
			}
			_scrollBar.Draw();

			for (int i = 0; i < _maxVisible; i++)
			{
				_availableTechsToResearchButtons[i].DrawToolTip();
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = _techDescription.MouseHover(x, y, frameDeltaTime);
			for (int i = 0; i < _maxVisible; i++)
			{
				result = _availableTechsToResearchButtons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			if (_scrollBar.MouseHover(x, y, frameDeltaTime))
			{
				RefreshTechButtons();
				result = true;
			}
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = _techDescription.MouseDown(x, y);
			for (int i = 0; i < _maxVisible; i++)
			{
				result = _availableTechsToResearchButtons[i].MouseDown(x, y) || result;
			}
			result = _scrollBar.MouseDown(x, y) || result;

			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			if (_techDescription.MouseUp(x, y))
			{
				return true;
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				if (_availableTechsToResearchButtons[i].MouseUp(x, y))
				{
					switch (_currentTechField)
					{
						case TechField.COMPUTER:
							_currentEmpire.TechnologyManager.WhichComputerBeingResearched = _availableTopics[_currentTechField][i + _scrollBar.TopIndex];
							break;
						case TechField.CONSTRUCTION:
							_currentEmpire.TechnologyManager.WhichConstructionBeingResearched = _availableTopics[_currentTechField][i + _scrollBar.TopIndex];
							break;
						case TechField.FORCE_FIELD:
							_currentEmpire.TechnologyManager.WhichForceFieldBeingResearched = _availableTopics[_currentTechField][i + _scrollBar.TopIndex];
							break;
						case TechField.PLANETOLOGY:
							_currentEmpire.TechnologyManager.WhichPlanetologyBeingResearched = _availableTopics[_currentTechField][i + _scrollBar.TopIndex];
							break;
						case TechField.PROPULSION:
							_currentEmpire.TechnologyManager.WhichPropulsionBeingResearched = _availableTopics[_currentTechField][i + _scrollBar.TopIndex];
							break;
						case TechField.WEAPON:
							_currentEmpire.TechnologyManager.WhichWeaponBeingResearched = _availableTopics[_currentTechField][i + _scrollBar.TopIndex];
							break;
					}
					_availableTopics.Remove(_currentTechField);
					if (_availableTopics.Count > 0)
					{
						LoadNextTech();
					}
					else if (Completed != null)
					{
						Completed();
					}
					return true;
				}
			}
			if (_scrollBar.MouseUp(x, y))
			{
				RefreshTechButtons();
				return true;
			}
			return false;
		}

		public void LoadEmpire(Empire empire, List<TechField> fields)
		{
			_currentEmpire = empire;
			_discoveredTechs.Clear();
			_fieldsNeedingNewTopics = new List<TechField>(fields);
			_availableTopics.Clear();

			foreach (var techField in _fieldsNeedingNewTopics)
			{
				switch (techField)
				{
					case TechField.COMPUTER:
					{
						_availableTopics.Add(techField, new List<Technology>());
						if (empire.TechnologyManager.WhichComputerBeingResearched != null)
						{
							_discoveredTechs.Add(empire.TechnologyManager.WhichComputerBeingResearched);
							empire.TechnologyManager.WhichComputerBeingResearched = null;
						}
						if (empire.TechnologyManager.UnresearchedComputerTechs.Count == 0)
						{
							break;
						}
						//Now find the next tier of techs
						int highestTech = 0;
						foreach (var tech in empire.TechnologyManager.ResearchedComputerTechs)
						{
							if (tech.TechLevel > highestTech)
							{
								highestTech = tech.TechLevel;
							}
						}
						highestTech = highestTech == 1 ? 5 : (((highestTech / 5) + (highestTech % 5 > 0 ? 2 : 1)) * 5);
						foreach (var tech in empire.TechnologyManager.UnresearchedComputerTechs)
						{
							if (tech.TechLevel <= highestTech)
							{
								_availableTopics[techField].Add(tech);
							}
						}
					} break;
					case TechField.CONSTRUCTION:
						{
							_availableTopics.Add(techField, new List<Technology>());
							if (empire.TechnologyManager.WhichConstructionBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichConstructionBeingResearched);
								empire.TechnologyManager.WhichConstructionBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedConstructionTechs.Count == 0)
							{
								break;
							}
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedConstructionTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = highestTech == 1 ? 5 : (((highestTech / 5) + (highestTech % 5 > 0 ? 2 : 1)) * 5);
							foreach (var tech in empire.TechnologyManager.UnresearchedConstructionTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
					case TechField.FORCE_FIELD:
						{
							_availableTopics.Add(techField, new List<Technology>());
							if (empire.TechnologyManager.WhichForceFieldBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichForceFieldBeingResearched);
								empire.TechnologyManager.WhichForceFieldBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedForceFieldTechs.Count == 0)
							{
								break;
							}
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedForceFieldTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = highestTech == 1 ? 5 : (((highestTech / 5) + (highestTech % 5 > 0 ? 2 : 1)) * 5);
							foreach (var tech in empire.TechnologyManager.UnresearchedForceFieldTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
					case TechField.PLANETOLOGY:
						{
							_availableTopics.Add(techField, new List<Technology>());
							if (empire.TechnologyManager.WhichPlanetologyBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichPlanetologyBeingResearched);
								empire.TechnologyManager.WhichPlanetologyBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedPlanetologyTechs.Count == 0)
							{
								break;
							}
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedPlanetologyTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = highestTech == 1 ? 5 : (((highestTech / 5) + (highestTech % 5 > 0 ? 2 : 1)) * 5);
							foreach (var tech in empire.TechnologyManager.UnresearchedPlanetologyTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
					case TechField.PROPULSION:
						{
							_availableTopics.Add(techField, new List<Technology>());
							if (empire.TechnologyManager.WhichPropulsionBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichPropulsionBeingResearched);
								empire.TechnologyManager.WhichPropulsionBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedPropulsionTechs.Count == 0)
							{
								break;
							}
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedPropulsionTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = highestTech == 1 ? 5 : (((highestTech / 5) + (highestTech % 5 > 0 ? 2 : 1)) * 5);
							foreach (var tech in empire.TechnologyManager.UnresearchedPropulsionTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
					case TechField.WEAPON:
						{
							_availableTopics.Add(techField, new List<Technology>());
							if (empire.TechnologyManager.WhichWeaponBeingResearched != null)
							{
								_discoveredTechs.Add(empire.TechnologyManager.WhichWeaponBeingResearched);
								empire.TechnologyManager.WhichWeaponBeingResearched = null;
							}
							if (empire.TechnologyManager.UnresearchedWeaponTechs.Count == 0)
							{
								break;
							}
							//Now find the next tier of techs
							int highestTech = 0;
							foreach (var tech in empire.TechnologyManager.ResearchedWeaponTechs)
							{
								if (tech.TechLevel > highestTech)
								{
									highestTech = tech.TechLevel;
								}
							}
							highestTech = highestTech == 1 ? 5 : (((highestTech / 5) + (highestTech % 5 > 0 ? 2 : 1)) * 5);
							foreach (var tech in empire.TechnologyManager.UnresearchedWeaponTechs)
							{
								if (tech.TechLevel <= highestTech)
								{
									_availableTopics[techField].Add(tech);
								}
							}
						} break;
				}
			}
			LoadNextTech();
		}

		private void LoadNextTech()
		{
			//Go in order from Computer, Construction, Force Field, Planetology, Propulsion, to Weapon
			if (_availableTopics.ContainsKey(TechField.COMPUTER))
			{
				_currentTechField = TechField.COMPUTER;
				//Check to see if there's a researched item
				bool hasDiscovered = false;
				foreach (var researchedItem in _discoveredTechs)
				{
					if (_currentEmpire.TechnologyManager.ResearchedComputerTechs.Contains(researchedItem))
					{
						_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
						_techDescription.SetText(researchedItem.TechName + "\n\r\n\r" + researchedItem.TechDescription);
						hasDiscovered = true;
						break;
					}
				}
				if (!hasDiscovered)
				{
					_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
					_techDescription.SetText("Computer technologies help with increasing number of factories, better scanners, improving your attack and missile defense on ships, and spying efforts benefits from higher computer tech level.");
				}
			}
			else if (_availableTopics.ContainsKey(TechField.CONSTRUCTION))
			{
				_currentTechField = TechField.CONSTRUCTION;
				//Check to see if there's a researched item
				bool hasDiscovered = false;
				foreach (var researchedItem in _discoveredTechs)
				{
					if (_currentEmpire.TechnologyManager.ResearchedConstructionTechs.Contains(researchedItem))
					{
						_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
						_techDescription.SetText(researchedItem.TechName + "\n\r\n\r" + researchedItem.TechDescription);
						hasDiscovered = true;
						break;
					}
				}
				if (!hasDiscovered)
				{
					_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
					_techDescription.SetText("Construction technologies gives you better armor, cheaper factories, reduced pollution, and higher construction tech levels gives you more room on ships.");
				}
			}
			else if (_availableTopics.ContainsKey(TechField.FORCE_FIELD))
			{
				_currentTechField = TechField.FORCE_FIELD;
				//Check to see if there's a researched item
				bool hasDiscovered = false;
				foreach (var researchedItem in _discoveredTechs)
				{
					if (_currentEmpire.TechnologyManager.ResearchedForceFieldTechs.Contains(researchedItem))
					{
						_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
						_techDescription.SetText(researchedItem.TechName + "\n\r\n\r" + researchedItem.TechDescription);
						hasDiscovered = true;
						break;
					}
				}
				if (!hasDiscovered)
				{
					_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
					_techDescription.SetText("Force field technologies gives you better shields, as well as planetary shields and nifty special items.");
				}
			}
			else if (_availableTopics.ContainsKey(TechField.PLANETOLOGY))
			{
				_currentTechField = TechField.PLANETOLOGY;
				//Check to see if there's a researched item
				bool hasDiscovered = false;
				foreach (var researchedItem in _discoveredTechs)
				{
					if (_currentEmpire.TechnologyManager.ResearchedPlanetologyTechs.Contains(researchedItem))
					{
						_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
						_techDescription.SetText(researchedItem.TechName + "\n\r\n\r" + researchedItem.TechDescription);
						hasDiscovered = true;
						break;
					}
				}
				if (!hasDiscovered)
				{
					_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
					_techDescription.SetText("Planetology technologies gives you terraforming and bigger planets, cheaper pollution cleanup, as well as expanding the number of planets you can colonize.  Also includes biological warfare.");
				}
			}
			else if (_availableTopics.ContainsKey(TechField.PROPULSION))
			{
				_currentTechField = TechField.PROPULSION;
				//Check to see if there's a researched item
				bool hasDiscovered = false;
				foreach (var researchedItem in _discoveredTechs)
				{
					if (_currentEmpire.TechnologyManager.ResearchedPropulsionTechs.Contains(researchedItem))
					{
						_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
						_techDescription.SetText(researchedItem.TechName + "\n\r\n\r" + researchedItem.TechDescription);
						hasDiscovered = true;
						break;
					}
				}
				if (!hasDiscovered)
				{
					_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
					_techDescription.SetText("Propulsion technologies gives you faster engines, expanded range, and powerful special equipment.");
				}
			}
			else if (_availableTopics.ContainsKey(TechField.WEAPON))
			{
				_currentTechField = TechField.WEAPON;
				//Check to see if there's a researched item
				bool hasDiscovered = false;
				foreach (var researchedItem in _discoveredTechs)
				{
					if (_currentEmpire.TechnologyManager.ResearchedWeaponTechs.Contains(researchedItem))
					{
						_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
						_techDescription.SetText(researchedItem.TechName + "\n\r\n\r" + researchedItem.TechDescription);
						hasDiscovered = true;
						break;
					}
				}
				if (!hasDiscovered)
				{
					_techIcon = SpriteManager.GetSprite("RandomRace", _gameMain.Random);
					_techDescription.SetText("Weapon technologies gives you weapons. A lot of weapons.");
				}
			}
			if (_availableTopics[_currentTechField].Count > 0)
			{
				if (_availableTopics[_currentTechField].Count > _availableTechsToResearchButtons.Length)
				{
					_maxVisible = _availableTechsToResearchButtons.Length;
					_scrollBar.SetEnabledState(true);
					_scrollBar.SetAmountOfItems(_availableTopics[_currentTechField].Count);
				}
				else
				{
					_maxVisible = _availableTopics[_currentTechField].Count;
					_scrollBar.SetAmountOfItems(_availableTechsToResearchButtons.Length);
					_scrollBar.SetEnabledState(false);
				}
				RefreshTechButtons();
			}
		}

		private void RefreshTechButtons()
		{
			var techManager = _gameMain.EmpireManager.CurrentEmpire.TechnologyManager;
			for (int i = 0; i < _maxVisible; i++)
			{
				_availableTechsToResearchButtons[i].SetText(_availableTopics[_currentTechField][i + _scrollBar.TopIndex].TechName);
				_availableTechsToResearchButtons[i].SetToolTipText(_availableTopics[_currentTechField][i + _scrollBar.TopIndex].TechDescription);
				_researchCosts[i].SetText(string.Format("{0:0} RPs", _availableTopics[_currentTechField][i + _scrollBar.TopIndex].ResearchPoints * TechnologyManager.COST_MODIFIER * techManager.RaceModifiers[_currentTechField]));
			}
		}
	}
}
