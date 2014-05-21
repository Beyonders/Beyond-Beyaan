using System;
using System.Collections.Generic;

namespace Beyond_Beyaan.Screens
{
	public enum EquipmentType { COMPUTER, SHIELD, ARMOR, ECM, ENGINE, MANEUVER, WEAPON, SPECIAL }

	public class EquipmentSelection : WindowInterface
	{
		private const int LABEL_WIDTH = 200;
		private const int COLUMN_WIDTH = 100;

		private BBStretchButton[] _buttons;
		private List<BBLabel[]> _columnValues;
		private List<BBLabel> _columnNames;

		private List<Equipment> _availableEquipments;
		private BBScrollBar _scrollBar;
		private int _maxVisible;
		private bool _scrollBarVisible;
		private int _numOfColumnsVisible;
		private int _middleX;
		private int _middleY;

		private Ship _shipDesign;
		private EquipmentType _equipmentType;
		private Dictionary<TechField, int> _techLevels;
		private float _spacePerPower;
		private float _costPerPower;
		private int _slotNum; //Used for specifying which weapon or special to replace, otherwise unused

		public Action<Equipment, int> OnSelectEquipment;
		public Action<int> OnSelectManeuver;

		public bool Initialize(GameMain gameMain, out string reason)
		{
			_middleX = gameMain.ScreenWidth / 2;
			_middleY = gameMain.ScreenHeight / 2;
			if (!base.Initialize((gameMain.ScreenWidth / 2) - 210, (gameMain.ScreenHeight / 2) - 230, 420, 460, StretchableImageType.ThinBorderBG, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			_buttons = new BBStretchButton[10];
			_columnValues = new List<BBLabel[]>();
			_columnNames = new List<BBLabel>();
			_maxVisible = 0;

			for (int i = 0; i < 10; i++)
			{
				_buttons[i] = new BBStretchButton();
				if (!_buttons[i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, _xPos + 10, _yPos + 50 + i * 40, 380, 40, gameMain.Random, out reason))
				{
					return false;
				}
				if (!_buttons[i].SetToolTip("EquipmentSelection" + i, string.Empty, gameMain.ScreenWidth, gameMain.ScreenHeight, gameMain.Random, out reason))
				{
					return false;
				}
			}
			_scrollBar = new BBScrollBar();
			if (!_scrollBar.Initialize(_xPos + 390, _yPos + 50, 400, 10, 10, false, false, gameMain.Random, out reason))
			{
				return false;
			}
			_maxVisible = 0;
			_numOfColumnsVisible = 0;
			_scrollBarVisible = false;
			return true;
		}

		public void LoadEquipments(Ship shipDesign, EquipmentType equipmentType, int slotNum, List<Technology> technologies, Dictionary<TechField, int> techLevels, float spacePerPower, float costPerPower)
		{
			_slotNum = slotNum;
			_shipDesign = shipDesign;
			_techLevels = techLevels;
			_equipmentType = equipmentType;
			_availableEquipments = new List<Equipment>();
			_spacePerPower = spacePerPower;
			_costPerPower = costPerPower;
			switch (_equipmentType)
			{
				case EquipmentType.ARMOR:
					LoadArmor(technologies);
					break;
				case EquipmentType.COMPUTER:
					LoadComputers(technologies);
					break;
				case EquipmentType.ECM:
					LoadECM(technologies);
					break;
				case EquipmentType.ENGINE:
					LoadEngines(technologies);
					break;
				case EquipmentType.MANEUVER:
					break;
				case EquipmentType.SHIELD:
					LoadShields(technologies);
					break;
				case EquipmentType.SPECIAL:
					LoadSpecials(technologies);
					break;
				case EquipmentType.WEAPON:
					LoadWeapons(technologies);
					break;
			}
			if (_availableEquipments.Count > 10)
			{
				_maxVisible = 10;
				_scrollBarVisible = true;
				_scrollBar.SetAmountOfItems(_availableEquipments.Count);
			}
			else
			{
				_maxVisible = _availableEquipments.Count;
				_scrollBarVisible = false;
				_scrollBar.TopIndex = 0;
			}
			SetControlsAndWindow();
			RefreshLabels();
		}
		public void LoadEquipments(Ship shipDesign, EquipmentType equipmentType, int currentManeuver, Dictionary<TechField, int> techLevels, float spacePerPower, float costPerPower)
		{
			_shipDesign = shipDesign;
			_techLevels = techLevels;
			_equipmentType = equipmentType;
			_availableEquipments = new List<Equipment>();
			_spacePerPower = spacePerPower;
			_costPerPower = costPerPower;
			_maxVisible = _shipDesign.Engine.Key.Technology.ManeuverSpeed; //Never more than 9, which is less than max of 10
			_scrollBarVisible = false;
			_scrollBar.TopIndex = 0;
			_numOfColumnsVisible = 4;
			SetControlsAndWindow();
			RefreshLabels();
		}

		private static Equipment GetEmptyEquipment()
		{
			return new Equipment(null, false);
		}

		private void LoadArmor(List<Technology> _technologies)
		{
			foreach (var tech in _technologies)
			{
				Equipment armorI = new Equipment(tech, false);
				Equipment armorII = new Equipment(tech, true);

				_availableEquipments.Add(armorI);
				_availableEquipments.Add(armorII);
			}
			_numOfColumnsVisible = 2;
		}

		private void LoadShields(List<Technology> _technologies)
		{
			_availableEquipments.Add(GetEmptyEquipment());
			foreach (var tech in _technologies)
			{
				var shield = new Equipment(tech, false);
				_availableEquipments.Add(shield);
			}
			_numOfColumnsVisible = 4;
		}

		private void LoadComputers(List<Technology> _technologies)
		{
			_availableEquipments.Add(GetEmptyEquipment());
			foreach (var tech in _technologies)
			{
				var computer = new Equipment(tech, false);
				_availableEquipments.Add(computer);
			}
			_numOfColumnsVisible = 4;
		}

		private void LoadECM(List<Technology> _technologies)
		{
			_availableEquipments.Add(GetEmptyEquipment());
			foreach (var tech in _technologies)
			{
				var ecm = new Equipment(tech, false);
				_availableEquipments.Add(ecm);
			}
			_numOfColumnsVisible = 4;
		}

		private void LoadEngines(List<Technology> _technologies)
		{
			foreach (var tech in _technologies)
			{
				var engine = new Equipment(tech, false);
				_availableEquipments.Add(engine);
			}
			_numOfColumnsVisible = 4;
		}

		private void LoadWeapons(List<Technology> _technologies)
		{
			_availableEquipments.Add(GetEmptyEquipment());
			foreach (var tech in _technologies)
			{
				var weapon = new Equipment(tech, false);
				_availableEquipments.Add(weapon);
				if (!string.IsNullOrEmpty(tech.TechSecondaryName) || tech.WeaponType == Technology.MISSILE_WEAPON)
				{
					weapon = new Equipment(tech, true);
					_availableEquipments.Add(weapon);
				}
			}
			_numOfColumnsVisible = 6;
		}
		private void LoadSpecials(List<Technology> _technologies)
		{
			_availableEquipments.Add(GetEmptyEquipment());
			foreach (var tech in _technologies)
			{
				var special = new Equipment(tech, false);
				_availableEquipments.Add(special);
			}
			_numOfColumnsVisible = 4;
		}

		private void SetControlsAndWindow()
		{
			_columnValues.Clear();
			int width = LABEL_WIDTH + _numOfColumnsVisible * COLUMN_WIDTH + (_scrollBarVisible ? 16 : 0);
			int height = 40 + _maxVisible * 40;
			int left = (_middleX - (width / 2));
			int top = (_middleY - (height / 2));
			string reason; //Unused, since at this point we've already initialized at least once, meaning everything is set up correctly
			_columnNames.Clear();
			_columnNames.Add(new BBLabel());
			_columnNames[0].Initialize(left + 5, top + 10, "Name", System.Drawing.Color.White, out reason);
			_columnValues.Add(new BBLabel[_maxVisible]);
			for (int i = 0; i < _maxVisible; i++)
			{
				_buttons[i].MoveTo(left, top + 40 + (i * 40));
				_buttons[i].ResizeButton(LABEL_WIDTH + _numOfColumnsVisible * COLUMN_WIDTH, 40);
				_columnValues[0][i] = new BBLabel();
				_columnValues[0][i].Initialize(left + 5, top + 50 + (i * 40), string.Empty, System.Drawing.Color.White, out reason);
			}
			left += LABEL_WIDTH;
			for (int i = 1; i <= _numOfColumnsVisible; i++)
			{
				_columnNames.Add(new BBLabel());
				_columnNames[i].Initialize(left + ((i - 1) * COLUMN_WIDTH) + 5, top + 10, string.Empty, System.Drawing.Color.White, out reason);
				_columnValues.Add(new BBLabel[_maxVisible]);
				for (int j = 0; j < _maxVisible; j++)
				{
					_columnValues[i][j] = new BBLabel();
					_columnValues[i][j].Initialize(left + ((i - 1) * COLUMN_WIDTH) + 5, top + 50 + (j * 40), string.Empty, System.Drawing.Color.White, out reason);
				}
			}
			//Move and resize the window to fit
			_xPos = (_gameMain.ScreenWidth / 2) - (width / 2) - 10;
			_yPos = (_gameMain.ScreenHeight / 2) - (height / 2) - 10;
			_backGroundImage.Resize(width + 20, height + 20);
			_backGroundImage.MoveTo(_xPos, _yPos);
			_scrollBar.MoveTo(left + _numOfColumnsVisible * COLUMN_WIDTH, top + 40);

			switch (_equipmentType)
			{
				case EquipmentType.ARMOR:
					{
						_columnNames[0].SetText("Armor Type");
						_columnNames[1].SetText("Cost");
						_columnNames[2].SetText("Space");
					} break;
				case EquipmentType.COMPUTER:
					{
						_columnNames[0].SetText("Computer Type");
						_columnNames[1].SetText("Cost");
						_columnNames[2].SetText("Size");
						_columnNames[3].SetText("Power");
						_columnNames[4].SetText("Space");
					} break;
				case EquipmentType.SHIELD:
					{
						_columnNames[0].SetText("Shield Type");
						_columnNames[1].SetText("Cost");
						_columnNames[2].SetText("Size");
						_columnNames[3].SetText("Power");
						_columnNames[4].SetText("Space");
					} break;
				case EquipmentType.ECM:
					{
						_columnNames[0].SetText("ECM Type");
						_columnNames[1].SetText("Cost");
						_columnNames[2].SetText("Size");
						_columnNames[3].SetText("Power");
						_columnNames[4].SetText("Space");
					} break;
				case EquipmentType.ENGINE:
					{
						_columnNames[0].SetText("Engine Type");
						_columnNames[1].SetText("Cost");
						_columnNames[2].SetText("Size");
						_columnNames[3].SetText("# Required");
						_columnNames[4].SetText("Space");
					} break;
				case EquipmentType.MANEUVER:
					{
						_columnNames[0].SetText("Maneuver Level");
						_columnNames[1].SetText("Speed");
						_columnNames[2].SetText("Cost");
						_columnNames[3].SetText("Power");
						_columnNames[4].SetText("Space");
					} break;
				case EquipmentType.WEAPON:
					{
						_columnNames[1].SetText("Damage");
						_columnNames[2].SetText("Cost");
						_columnNames[3].SetText("Size");
						_columnNames[4].SetText("Power");
						_columnNames[5].SetText("Space");
						_columnNames[6].SetText("Max Mounts");
					} break;
			}
		}

		private void RefreshLabels()
		{
			float availableSpace = _shipDesign.TotalSpace - _shipDesign.SpaceUsed;
			//Add back the available space used by current equipment, if any, to accurately reflect the option of changing current equipment to another equipment
			switch (_equipmentType)
			{
				case EquipmentType.ARMOR:
					{
						availableSpace += _shipDesign.Armor.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower);
					} break;
				case EquipmentType.COMPUTER:
					{
						if (_shipDesign.Computer != null)
						{
							availableSpace += _shipDesign.Computer.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower);
						}
					} break;
				case EquipmentType.SHIELD:
					{
						if (_shipDesign.Shield != null)
						{
							availableSpace += _shipDesign.Shield.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower);
						}
					} break;
				case EquipmentType.ECM:
					{
						if (_shipDesign.ECM != null)
						{
							availableSpace += _shipDesign.ECM.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower);
						}
					} break;
				case EquipmentType.ENGINE:
					{
						availableSpace += _shipDesign.Engine.Key.GetSize(_techLevels, _shipDesign.Size) * _shipDesign.Engine.Value;
					} break;
				case EquipmentType.MANEUVER:
					{
						availableSpace += PowerRequiredForManeuver(_shipDesign.ManeuverSpeed) * _spacePerPower;
					} break;
				case EquipmentType.SPECIAL:
					{
						if (_shipDesign.Specials[_slotNum] != null)
						{
							availableSpace += _shipDesign.Specials[_slotNum].GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower);
						}
					} break;
				case EquipmentType.WEAPON:
					{
						if (_shipDesign.Weapons[_slotNum].Key != null)
						{
							availableSpace += _shipDesign.Weapons[_slotNum].Key.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower) * _shipDesign.Weapons[_slotNum].Value;
						}
					} break;
			}

			if (_equipmentType == EquipmentType.MANEUVER)
			{
				//Unique condition since Maneuver isn't actually an equipment
				for (int i = 0; i < _shipDesign.Engine.Key.Technology.ManeuverSpeed; i++)
				{
					int powerReq = PowerRequiredForManeuver(i + 1);
					_columnValues[0][i].SetText(string.Format("Class {0}", i + 1));
					_columnValues[1][i].SetText(string.Format("{0}", (i + 2) / 2));
					_columnValues[2][i].SetText(string.Format("{0:0.0}", powerReq * _costPerPower));
					_columnValues[3][i].SetText(string.Format("{0:0.0}", powerReq));
					_columnValues[4][i].SetText(string.Format("{0:0.0}", powerReq * _spacePerPower));
					_buttons[i].SetToolTipText("Maneuver Rating of " + (i + 1));
					if (powerReq * _spacePerPower > availableSpace)
					{
						// TODO: Add restrictions for specials, i.e. having an colony special disables all other colony options.  Having an special restricts that from being available for other slots.  Etc.

						_buttons[i].Enabled = false;
						for (int j = 0; j <= _numOfColumnsVisible; j++)
						{
							_columnValues[j][i].SetColor(System.Drawing.Color.Tan, System.Drawing.Color.Empty);
						}
					}
					else
					{
						_buttons[i].Enabled = true;
						for (int j = 0; j <= _numOfColumnsVisible; j++)
						{
							_columnValues[j][i].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
						}
					}
				}
				return;
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				SetText(i);
				if (_availableEquipments[i + _scrollBar.TopIndex].Technology == null)
				{
					//None is always available
					_buttons[i].Enabled = true;
					for (int j = 0; j <= _numOfColumnsVisible; j++)
					{
						_columnValues[j][i].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
					}
				}
				else if (_availableEquipments[i + _scrollBar.TopIndex].GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower) > availableSpace)
				{
					_buttons[i].Enabled = false;
					for (int j = 0; j <= _numOfColumnsVisible; j++)
					{
						_columnValues[j][i].SetColor(System.Drawing.Color.Tan, System.Drawing.Color.Empty);
					}
				}
				else
				{
					_buttons[i].Enabled = true;
					for (int j = 0; j <= _numOfColumnsVisible; j++)
					{
						_columnValues[j][i].SetColor(System.Drawing.Color.White, System.Drawing.Color.Empty);
					}
				}
			}
		}

		private void SetText(int whichRow)
		{
			var equipment = _availableEquipments[whichRow + _scrollBar.TopIndex];
			if (equipment.Technology == null)
			{
				_columnValues[0][whichRow].SetText("None");
				_buttons[whichRow].SetToolTipText("None");
			}
			else
			{
				_columnValues[0][whichRow].SetText(equipment.DisplayName);
				_buttons[whichRow].SetToolTipText(equipment.Technology.TechDescription);
			}
			switch (_equipmentType)
			{
				case EquipmentType.ARMOR:
					{
						_columnValues[1][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualCost(_techLevels, _shipDesign.Size, _costPerPower)));
						_columnValues[2][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower)));
					} break;
				case EquipmentType.COMPUTER:
					{
						if (equipment.Technology == null)
						{
							_columnValues[1][whichRow].SetText("0");
							_columnValues[2][whichRow].SetText("0");
							_columnValues[3][whichRow].SetText("0");
							_columnValues[4][whichRow].SetText("0");
						}
						else
						{
							_columnValues[1][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualCost(_techLevels, _shipDesign.Size, _costPerPower)));
							_columnValues[2][whichRow].SetText(string.Format("{0:0.0}", equipment.GetSize(_techLevels, _shipDesign.Size)));
							_columnValues[3][whichRow].SetText(string.Format("{0:0.0}", equipment.GetPower(_shipDesign.Size)));
							_columnValues[4][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower)));
						}
					} break;
				case EquipmentType.SHIELD:
					{
						if (equipment.Technology == null)
						{
							_columnValues[1][whichRow].SetText("0");
							_columnValues[2][whichRow].SetText("0");
							_columnValues[3][whichRow].SetText("0");
							_columnValues[4][whichRow].SetText("0");
						}
						else
						{
							_columnValues[1][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualCost(_techLevels, _shipDesign.Size, _costPerPower)));
							_columnValues[2][whichRow].SetText(string.Format("{0:0.0}", equipment.GetSize(_techLevels, _shipDesign.Size)));
							_columnValues[3][whichRow].SetText(string.Format("{0:0.0}", equipment.GetPower(_shipDesign.Size)));
							_columnValues[4][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower)));
						}
					} break;
				case EquipmentType.ECM:
					{
						if (equipment.Technology == null)
						{
							_columnValues[1][whichRow].SetText("0");
							_columnValues[2][whichRow].SetText("0");
							_columnValues[3][whichRow].SetText("0");
							_columnValues[4][whichRow].SetText("0");
						}
						else
						{
							_columnValues[1][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualCost(_techLevels, _shipDesign.Size, _costPerPower)));
							_columnValues[2][whichRow].SetText(string.Format("{0:0.0}", equipment.GetSize(_techLevels, _shipDesign.Size)));
							_columnValues[3][whichRow].SetText(string.Format("{0:0.0}", equipment.GetPower(_shipDesign.Size)));
							_columnValues[4][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower)));
						}
					} break;
				case EquipmentType.ENGINE:
					{
						float amountReq = _shipDesign.PowerUsed / (equipment.Technology.Speed * 10);
						float size = equipment.GetSize(_techLevels, _shipDesign.Size);
						_columnValues[1][whichRow].SetText(string.Format("{0:0.0}", equipment.GetCost(_techLevels, _shipDesign.Size)));
						_columnValues[2][whichRow].SetText(string.Format("{0:0.0}", size));
						_columnValues[3][whichRow].SetText(string.Format("{0:0.0}", amountReq));
						_columnValues[4][whichRow].SetText(string.Format("{0:0.0}", amountReq * size));
					} break;
				case EquipmentType.SPECIAL:
					{
						if (equipment.Technology == null)
						{
							_columnValues[1][whichRow].SetText("0");
							_columnValues[2][whichRow].SetText("0");
							_columnValues[3][whichRow].SetText("0");
							_columnValues[4][whichRow].SetText("0");
						}
						else
						{
							_columnValues[1][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualCost(_techLevels, _shipDesign.Size, _costPerPower)));
							_columnValues[2][whichRow].SetText(string.Format("{0:0.0}", equipment.GetSize(_techLevels, _shipDesign.Size)));
							_columnValues[3][whichRow].SetText(string.Format("{0:0.0}", equipment.GetPower(_shipDesign.Size)));
							_columnValues[4][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower)));
						}
					} break;
				case EquipmentType.WEAPON:
				{
					if (equipment.Technology == null)
					{
						_columnValues[1][whichRow].SetText("0");
						_columnValues[2][whichRow].SetText("0");
						_columnValues[3][whichRow].SetText("0");
						_columnValues[4][whichRow].SetText("0");
						_columnValues[5][whichRow].SetText("0");
						_columnValues[6][whichRow].SetText("0");
					}
					else
					{
						float availableSpace = _shipDesign.TotalSpace - _shipDesign.SpaceUsed;
						if (_shipDesign.Weapons[_slotNum].Key != null)
						{
							availableSpace += _shipDesign.Weapons[_slotNum].Key.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower) * _shipDesign.Weapons[_slotNum].Value;
						}
						float actualSize = equipment.GetActualSize(_techLevels, _shipDesign.Size, _spacePerPower);
						if (equipment.Technology.WeaponType == Technology.BEAM_WEAPON || equipment.Technology.WeaponType == Technology.BOMB_WEAPON)
						{
							_columnValues[1][whichRow].SetText(string.Format("{0}-{1}", equipment.GetMinDamage(), equipment.GetMaxDamage()));
						}
						else if (equipment.Technology.WeaponType != Technology.BIOLOGICAL_WEAPON)
						{
							_columnValues[1][whichRow].SetText(string.Format("{0}", equipment.GetMaxDamage()));
						}
						else
						{
							_columnValues[1][whichRow].SetText(string.Format("{0} POP", equipment.Technology.BioWeapon));
						}
						_columnValues[2][whichRow].SetText(string.Format("{0:0.0}", equipment.GetActualCost(_techLevels, _shipDesign.Size, _costPerPower)));
						_columnValues[3][whichRow].SetText(string.Format("{0:0.0}", equipment.GetSize(_techLevels, _shipDesign.Size)));
						_columnValues[4][whichRow].SetText(string.Format("{0:0.0}", equipment.GetPower(_shipDesign.Size)));
						_columnValues[5][whichRow].SetText(string.Format("{0:0.0}", actualSize));
						_columnValues[6][whichRow].SetText(string.Format("{0}", (int)(availableSpace / actualSize)));
					}
				} break;
			}
		}

		private int PowerRequiredForManeuver(int maneuverAmount)
		{
			switch (_shipDesign.Size)
			{
				case Ship.SMALL:
				{
					return maneuverAmount * 2;
				}
				case Ship.MEDIUM:
				{
					return maneuverAmount * 15;
				}
				case Ship.LARGE:
				{
					return maneuverAmount * 100;
				}
				case Ship.HUGE:
				{
					return maneuverAmount * 700;
				}
			}
			return -1;
		}

		public override void Draw()
		{
			base.Draw();
			for (int i = 0; i < _maxVisible; i++)
			{
				_buttons[i].Draw();
				for (int j = 0; j <= _numOfColumnsVisible; j++)
				{
					_columnValues[j][i].Draw();
				}
			}
			for (int i = 0; i <= _numOfColumnsVisible; i++)
			{
				_columnNames[i].Draw();
			}
			for (int i = 0; i < _maxVisible; i++)
			{
				_buttons[i].DrawToolTip();
			}
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			bool result = false;
			for (int i = 0; i < _maxVisible; i++)
			{
				result = _buttons[i].MouseHover(x, y, frameDeltaTime) || result;
			}
			return result;
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = false;
			for (int i = 0; i < _maxVisible; i++)
			{
				result = _buttons[i].MouseDown(x, y) || result;
			}
			return result;
		}

		public override bool MouseUp(int x, int y)
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				if (_buttons[i].MouseUp(x, y))
				{
					if (_equipmentType == EquipmentType.MANEUVER)
					{
						if (OnSelectManeuver != null)
						{
							OnSelectManeuver(i + 1);
						}
					}
					if (OnSelectEquipment != null)
					{
						OnSelectEquipment(_availableEquipments[i + _scrollBar.TopIndex], _slotNum);
					}
					return true;
				}
			}
			return base.MouseUp(x, y);
		}
	}
}
