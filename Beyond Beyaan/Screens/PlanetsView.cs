using System;
using System.Collections.Generic;
using System.Drawing;

namespace Beyond_Beyaan.Screens
{
	public class PlanetsView : WindowInterface
	{
		public Action CloseWindow;
		public Action<StarSystem> CenterToSystem;

		private BBStretchButton[] _columnHeaders;
		private BBStretchButton[][] _columnCells;
		private BBScrollBar _scrollBar;

		private BBStretchableImage _expensesBackground;
		private BBStretchableImage _incomeBackground;
		private BBStretchableImage _reserves;

		private BBLabel _expenseTitle;
		private BBLabel _incomeTitle;

		private BBLabel _reservesLabel;
		private BBLabel _reservesAmount;
		private BBScrollBar _reserveSlider;

		private BBLabel _transferLabel;
		private BBLabel _transferAmount;
		private BBScrollBar _transferSlider;
		private BBStretchButton _transferReserves;

		private BBStretchButton[] _expenses;
		private BBStretchButton[] _incomes;
		private BBLabel[] _expenseLabels;
		private BBLabel[] _incomeLabels;

		private int _maxVisible;
		private int _selectedRow;

		#region Constructor
		public bool Initialize(GameMain gameMain, out string reason)
		{
			int x = (gameMain.ScreenWidth / 2) - 533;
			int y = (gameMain.ScreenHeight / 2) - 300;

			if (!Initialize(x, y, 1066, 600, StretchableImageType.MediumBorder, gameMain, false, gameMain.Random, out reason))
			{
				return false;
			}

			x += 20;
			y += 20;

			_columnHeaders = new BBStretchButton[8];
			for (int i = 0; i < _columnHeaders.Length; i++)
			{
				_columnHeaders[i] = new BBStretchButton();
			}
			_columnCells = new BBStretchButton[8][];
			for (int i = 0; i < _columnCells.Length; i++)
			{
				_columnCells[i] = new BBStretchButton[13];
				for (int j = 0; j < _columnCells[i].Length; j++)
				{
					_columnCells[i][j] = new BBStretchButton();
				}
			}

			if (!_columnHeaders[0].Initialize("Planet", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 280, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[0][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 280, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 280;
			if (!_columnHeaders[1].Initialize("Population", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 90, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[1][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 90, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 90;
			if (!_columnHeaders[2].Initialize("Buildings", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 90, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[2][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 90, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 90;
			if (!_columnHeaders[3].Initialize("Bases", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 80, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[3][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 80, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 80;
			if (!_columnHeaders[4].Initialize("Waste", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 80, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[4][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 80, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 80;
			if (!_columnHeaders[5].Initialize("Industry", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 80, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[5][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 80, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 80;
			if (!_columnHeaders[6].Initialize("Constructing", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 250, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[6][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 250, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}

			x += 250;
			if (!_columnHeaders[7].Initialize("Notes", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonBG, x, y, 60, 30, _gameMain.Random, out reason))
			{
				return false;
			}
			for (int i = 0; i < 13; i++)
			{
				if (!_columnCells[7][i].Initialize(string.Empty, ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x, y + 30 + (i * 30), 60, 30, _gameMain.Random, out reason))
				{
					return false;
				}
			}
			x += 60;
			_scrollBar = new BBScrollBar();
			if (!_scrollBar.Initialize(x, y + 30, 390, 13, 13, false, false, _gameMain.Random, out reason))
			{
				return false;
			}

			_expensesBackground = new BBStretchableImage();
			_incomeBackground = new BBStretchableImage();
			_reserves = new BBStretchableImage();

			_expenseTitle = new BBLabel();
			_incomeTitle = new BBLabel();

			_expenses = new BBStretchButton[4];
			_expenseLabels = new BBLabel[4];
			_incomes = new BBStretchButton[2];
			_incomeLabels = new BBLabel[2];
			for (int i = 0; i < 4; i++)
			{
				_expenses[i] = new BBStretchButton();
				_expenseLabels[i] = new BBLabel();
			}
			for (int i = 0; i < 2; i++)
			{
				_incomes[i] = new BBStretchButton();
				_incomeLabels[i] = new BBLabel();
			}

			x = (gameMain.ScreenWidth / 2) - 513;
			y = (gameMain.ScreenHeight / 2) + 143;

			if (!_expensesBackground.Initialize(x, y, 476, 140, StretchableImageType.ThinBorderBG, _gameMain.Random, out reason))
			{
				return false;
			}
			if (!_expenseTitle.Initialize(0, 0, "Expenses", Color.Gold, "LargeComputerFont", out reason))
			{
				return false;
			}
			_expenseTitle.MoveTo((int)(x + 238 - _expenseTitle.GetWidth() / 2), y + 5);
			if (!_expenses[0].Initialize("Ships", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 50,  228, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_expenses[0].SetTextColor(Color.Orange, Color.Empty);
			if (!_expenseLabels[0].Initialize(x + 228, y + 65, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_expenseLabels[0].SetAlignment(true);
			if (!_expenses[1].Initialize("Bases", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 90, 228, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_expenses[1].SetTextColor(Color.Orange, Color.Empty);
			if (!_expenseLabels[1].Initialize(x + 228, y + 105, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_expenseLabels[1].SetAlignment(true);
			if (!_expenses[2].Initialize("Spying", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 238, y + 50, 228, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_expenses[2].SetTextColor(Color.Orange, Color.Empty);
			if (!_expenseLabels[2].Initialize(x + 456, y + 65, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_expenseLabels[2].SetAlignment(true);
			if (!_expenses[3].Initialize("Security", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 238, y + 90, 228, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_expenses[3].SetTextColor(Color.Orange, Color.Empty);
			if (!_expenseLabels[3].Initialize(x + 456, y + 105, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_expenseLabels[3].SetAlignment(true);
			x += 476;

			if (!_incomeBackground.Initialize(x, y, 250, 140, StretchableImageType.ThinBorderBG, _gameMain.Random, out reason))
			{
				return false;
			}
			if (!_incomeTitle.Initialize(0, 0, "Incomes", Color.Gold, "LargeComputerFont", out reason))
			{
				return false;
			}
			if (!_incomes[0].Initialize("Planets", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 50, 230, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_incomes[0].SetTextColor(Color.Orange, Color.Empty);
			if (!_incomeLabels[0].Initialize(x + 230, y + 65, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_incomeLabels[0].SetAlignment(true);
			if (!_incomes[1].Initialize("Trade", ButtonTextAlignment.LEFT, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 90, 230, 40, _gameMain.Random, out reason))
			{
				return false;
			}
			_incomes[1].SetTextColor(Color.Orange, Color.Empty);
			if (!_incomeLabels[1].Initialize(x + 230, y + 105, string.Empty, Color.White, out reason))
			{
				return false;
			}
			_incomeLabels[1].SetAlignment(true);
			_incomeTitle.MoveTo((int)(x + 125 - _incomeTitle.GetWidth() / 2), y + 5);
			x += 250;

			if (!_reserves.Initialize(x, y, 300, 140, StretchableImageType.ThinBorderBG, _gameMain.Random, out reason))
			{
				return false;
			}
			_reserveSlider = new BBScrollBar();
			_reservesLabel = new BBLabel();
			_reservesAmount = new BBLabel();

			_transferSlider = new BBScrollBar();
			_transferLabel = new BBLabel();
			_transferAmount = new BBLabel();
			_transferReserves = new BBStretchButton();

			if (!_reservesLabel.Initialize(x + 10, y + 10, "Reserve:", Color.Orange, out reason))
			{
				return false;
			}
			if (!_reservesAmount.Initialize(x + 280, y + 10, string.Empty, Color.White, out reason))
			{
				return false;
			}
			if (!_reserveSlider.Initialize(x + 10, y + 33, 280, 0, 20, true, true, _gameMain.Random, out reason))
			{
				return false;
			}
			if (!_transferLabel.Initialize(x + 10, y + 51, "Amount to transfer:", Color.Orange, out reason))
			{
				return false;
			}
			if (!_transferAmount.Initialize(x + 280, y + 51, string.Empty, Color.White, out reason))
			{
				return false;
			}
			if (!_transferSlider.Initialize(x + 10, y + 72, 280, 0, 200, true, true, _gameMain.Random, out reason))
			{
				return false;
			}
			if (!_transferReserves.Initialize("Transfer reserves to selected planet", ButtonTextAlignment.CENTER, StretchableImageType.TinyButtonBG, StretchableImageType.TinyButtonFG, x + 10, y + 95, 280, 35, _gameMain.Random, out reason))
			{
				return false;
			}
			_reservesAmount.SetAlignment(true);
			_transferAmount.SetAlignment(true);

			return true;
		}
		#endregion

		public void Load()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			var planets = currentEmpire.PlanetManager.Planets;
			if (planets.Count > 13)
			{
				_maxVisible = 13;
				_scrollBar.SetEnabledState(true);
				_scrollBar.SetAmountOfItems(planets.Count);
			}
			else
			{
				_maxVisible = planets.Count;
				_scrollBar.SetEnabledState(false);
				_scrollBar.SetAmountOfItems(13);
			}
			_scrollBar.TopIndex = 0;
			RefreshPlanets(planets);
			
			_expenseLabels[0].SetText(string.Format("{0:0.0}% ({1:0.0} BC)", currentEmpire.ShipMaintenancePercentage * 100, currentEmpire.ShipMaintenance));
			_expenseLabels[1].SetText(string.Format("{0:0.0}% ({1:0.0} BC)", currentEmpire.BaseMaintenancePercentage * 100, currentEmpire.BaseMaintenance));
			_expenseLabels[2].SetText(string.Format("{0:0.0}% ({1:0.0} BC)", currentEmpire.EspionageExpensePercentage * 100, currentEmpire.EspionageExpense));
			_expenseLabels[3].SetText(string.Format("{0:0.0}% ({1:0.0} BC)", currentEmpire.SecurityExpensePercentage * 100, currentEmpire.SecurityExpense));

			_incomeLabels[0].SetText(string.Format("{0:0.0} BC", currentEmpire.PlanetTotalProduction));
			_incomeLabels[1].SetText(string.Format("{0:0.0} BC", currentEmpire.TradeIncome));

			_selectedRow = 0;
			RefreshSelection();

			RefreshReserves();

			_transferSlider.TopIndex = 0;
			RefreshTransfer();
		}

		public override void Draw()
		{
			base.Draw();

			for (int i = 0; i < _columnHeaders.Length; i++)
			{
				_columnHeaders[i].Draw();
			}
			for (int i = 0; i < _columnCells.Length; i++)
			{
				for (int j = 0; j < _columnCells[i].Length; j++)
				{
					_columnCells[i][j].Draw();
				}
			}
			_expensesBackground.Draw();
			_incomeBackground.Draw();
			_reserves.Draw();
			_expenseTitle.Draw();
			_incomeTitle.Draw();
			_scrollBar.Draw();
			for (int i = 0; i < 4; i++)
			{
				_expenses[i].Draw();
				_expenseLabels[i].Draw();
			}
			for (int i = 0; i < 2; i++)
			{
				_incomes[i].Draw();
				_incomeLabels[i].Draw();
			}
			_reserveSlider.Draw();
			_reservesLabel.Draw();
			_reservesAmount.Draw();
			_transferSlider.Draw();
			_transferLabel.Draw();
			_transferAmount.Draw();
			_transferReserves.Draw();
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			int left = (_gameMain.ScreenWidth / 2) - 513;
			int top = (_gameMain.ScreenHeight / 2) - 250;
			if (x >= left && x < left + 1010 && y >= top && y < top + 390)
			{
				//When hovering over a column, that entire row is highlighted
				for (int i = 0; i < _maxVisible; i++)
				{
					_columnCells[0][i].MouseHover(left + 1, y, frameDeltaTime);
					_columnCells[1][i].MouseHover(left + 281, y, frameDeltaTime);
					_columnCells[2][i].MouseHover(left + 371, y, frameDeltaTime);
					_columnCells[3][i].MouseHover(left + 461, y, frameDeltaTime);
					_columnCells[4][i].MouseHover(left + 541, y, frameDeltaTime);
					_columnCells[5][i].MouseHover(left + 621, y, frameDeltaTime);
					_columnCells[6][i].MouseHover(left + 701, y, frameDeltaTime);
					_columnCells[7][i].MouseHover(left + 951, y, frameDeltaTime);
				}
			}
			if (_reserveSlider.MouseHover(x, y, frameDeltaTime))
			{
				_gameMain.EmpireManager.CurrentEmpire.TaxRate = _reserveSlider.TopIndex;
				RefreshReserves();
				return true;
			}
			if (_transferSlider.MouseHover(x, y, frameDeltaTime))
			{
				RefreshTransfer();
				return true;
			}
			_transferReserves.MouseHover(x, y, frameDeltaTime);
			return base.MouseHover(x, y, frameDeltaTime);
		}

		public override bool MouseDown(int x, int y)
		{
			int left = (_gameMain.ScreenWidth / 2) - 513;
			int top = (_gameMain.ScreenHeight / 2) - 250;
			if (x >= left && x < left + 1010 && y >= top && y < top + 390)
			{
				//When clicking a column, that entire row is clicked
				for (int i = 0; i < _maxVisible; i++)
				{
					_columnCells[0][i].MouseDown(left + 1, y);
					_columnCells[1][i].MouseDown(left + 281, y);
					_columnCells[2][i].MouseDown(left + 371, y);
					_columnCells[3][i].MouseDown(left + 461, y);
					_columnCells[4][i].MouseDown(left + 541, y);
					_columnCells[5][i].MouseDown(left + 621, y);
					_columnCells[6][i].MouseDown(left + 701, y);
					_columnCells[7][i].MouseDown(left + 951, y);
				}
			}
			_transferReserves.MouseDown(x, y);
			_reserveSlider.MouseDown(x, y);
			_transferSlider.MouseDown(x, y);
			return base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
			int left = (_gameMain.ScreenWidth / 2) - 513;
			int top = (_gameMain.ScreenHeight / 2) - 250;
			if (x >= left && x < left + 1010 && y >= top && y < top + 390)
			{
				//When clicking a column, that entire row is clicked
				for (int i = 0; i < _maxVisible; i++)
				{
					_columnCells[0][i].MouseUp(left + 1, y);
					_columnCells[1][i].MouseUp(left + 281, y);
					_columnCells[2][i].MouseUp(left + 371, y);
					_columnCells[3][i].MouseUp(left + 461, y);
					_columnCells[4][i].MouseUp(left + 541, y);
					_columnCells[5][i].MouseUp(left + 621, y);
					_columnCells[6][i].MouseUp(left + 701, y);
					if (_columnCells[7][i].MouseUp(left + 951, y))
					{
						if (_columnCells[7][i].DoubleClicked)
						{
							if (CenterToSystem != null)
							{
								CenterToSystem(
									_gameMain.EmpireManager.CurrentEmpire.PlanetManager.Planets[_selectedRow].System);
							}
						}
						else
						{
							_selectedRow = i + _scrollBar.TopIndex;
							RefreshSelection();
						}
					}
				}
			}
			if (_reserveSlider.MouseUp(x, y))
			{
				_gameMain.EmpireManager.CurrentEmpire.TaxRate = _reserveSlider.TopIndex;
				RefreshReserves();
				return true;
			}
			if (_transferSlider.MouseUp(x, y))
			{
				RefreshTransfer();
				return true;
			}
			if (_transferReserves.MouseUp(x, y))
			{
				var empire = _gameMain.EmpireManager.CurrentEmpire;
				var planet = empire.PlanetManager.Planets[_selectedRow];
				float amount = empire.Reserves * (_transferSlider.TopIndex / 200.0f);
				planet.Reserves += amount;
				empire.Reserves -= amount;
				_transferSlider.TopIndex = 0;
				RefreshReserves();
				RefreshTransfer();
				return true;
			}
			if (!base.MouseUp(x, y))
			{
				if (CloseWindow != null)
				{
					CloseWindow();
					return true;
				}
			}
			return false;
		}

		private void RefreshReserves()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			_reservesAmount.SetText(currentEmpire.TaxRate == 0
										? string.Format("{0:0.0} BC", currentEmpire.Reserves)
										: string.Format("{0:0.0} (+{1:0.0}) BC", currentEmpire.Reserves,
														currentEmpire.TaxExpenses / 2));
			var planets = currentEmpire.PlanetManager.Planets;
			for (int i = 0; i < _maxVisible; i++)
			{
				_columnCells[5][i + _scrollBar.TopIndex].SetText(((int)planets[i].ActualProduction).ToString());
			}
		}

		private void RefreshTransfer()
		{
			var currentEmpire = _gameMain.EmpireManager.CurrentEmpire;
			float amount = currentEmpire.Reserves;
			if (amount <= 0.0001)
			{
				_transferAmount.SetText("0.0 BC");
				_transferSlider.SetEnabledState(false);
				_transferReserves.Enabled = false;
			}
			else
			{
				float percentage = _transferSlider.TopIndex / 200.0f;
				amount *= percentage;
				_transferAmount.SetText(string.Format("{0:0.0} BC", amount));
				_transferSlider.SetEnabledState(true);
				_transferReserves.Enabled = true;
			}
		}

		private void RefreshSelection()
		{
			for (int i = 0; i < 13; i++)
			{
				if (i == _selectedRow)
				{
					for (int j = 0; j < 8; j++)
					{
						_columnCells[j][i].Selected = true;
					}
				}
				else
				{
					for (int j = 0; j < 8; j++)
					{
						_columnCells[j][i].Selected = false;
					}
				}
			}
		}

		public void RefreshPlanets(List<Planet> planets)
		{
			for (int i = 0; i < _maxVisible; i++)
			{
				_columnCells[0][i + _scrollBar.TopIndex].SetText(planets[i + _scrollBar.TopIndex].Name);
				_columnCells[1][i + _scrollBar.TopIndex].SetText(string.Format("{0:0.0}", planets[i + _scrollBar.TopIndex].TotalPopulation));
				_columnCells[2][i + _scrollBar.TopIndex].SetText(string.Format("{0:0.0}", planets[i + _scrollBar.TopIndex].Factories));
				_columnCells[3][i + _scrollBar.TopIndex].SetText(planets[i + _scrollBar.TopIndex].Bases.ToString());
				_columnCells[4][i + _scrollBar.TopIndex].SetText(string.Format("{0:0.0}", planets[i + _scrollBar.TopIndex].Waste));
				_columnCells[5][i + _scrollBar.TopIndex].SetText(((int)planets[i + _scrollBar.TopIndex].ActualProduction).ToString());
				_columnCells[6][i + _scrollBar.TopIndex].SetText(planets[i].ConstructionAmount > 0 ? planets[i + _scrollBar.TopIndex].ShipBeingBuilt.Name : string.Empty);
				for (int j = 0; j < 8; j++)
				{
					_columnCells[j][i].Enabled = true;
				}
				if (i + _scrollBar.TopIndex == _selectedRow)
				{
					for (int j = 0; j < 8; j++)
					{
						_columnCells[j][i].Selected = true;
					}
				}
				else
				{
					for (int j = 0; j < 8; j++)
					{
						_columnCells[j][i].Selected = true;
					}
				}
			}
			for (int i = _maxVisible; i < 13; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					_columnCells[j][i].Enabled = false;
				}
			}
		}
	}
}
