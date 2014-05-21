using GorgonLibrary.InputDevices;

namespace Beyond_Beyaan.Screens
{
	public class DiplomacyScreen : ScreenInterface
	{
		#region Constants
		/*const int TRADE = 0;
		const int RESEARCH = 1;
		const int ALLY = 2;
		const int UNALLY = 3;
		const int HARASS = 4;
		const int RECONCILE = 5;
		const int WAR = 6;
		const int SEND = 7;
		const int ACCEPT = 8;
		const int REJECT = 9;*/
		#endregion


		GameMain gameMain;
		//List<Contact> empiresInContact;

		/*ScrollBar empireScrollBar;
		ScrollBar[] spyEffortScrollbars;
		ScrollBar[] spyDefenseScrollbars;
		Button[] empireButtons;
		Label[] relationLabels;
		Label[] empireNameLabels;
		BBSprite[] avatars;
		BBSprite profile;
		int whichContactSelected;
		Button[] messageOptions;
		ComboBox listOfEmpires;
		List<SpriteName> spriteNames;
		private bool isViewingReceivedMessage;
		private int whichMessageToSend;
		private List<Empire> adjustedEmpiresForSelection;
		private Button[] incomingMessages;
		private Label IncomingMessageTextBox; //Replace with real text box when it's implemented

		int maxVisible;
		int x;
		int y;*/

		public bool Initialize(GameMain gameMain, out string reason)
		{
			this.gameMain = gameMain;
			/*x = (gameMain.ScreenWidth / 2) - 400;
			y = (gameMain.ScreenHeight / 2) - 300;
			empireButtons = new Button[4];
			spyEffortScrollbars = new ScrollBar[4];
			spyDefenseScrollbars = new ScrollBar[4];
			for (int i = 0; i < empireButtons.Length; i++)
			{
				empireButtons[i] = new Button(SpriteName.NormalBackgroundButton, SpriteName.NormalForegroundButton, string.Empty, x, y + (150 * i), 384, 150);
				spyEffortScrollbars[i] = new ScrollBar(x + 145, y + 45 + (150 * i), 16, 174, 1, 101, true, true, SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton,
					SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, SpriteName.SliderHorizontalBackgroundButton, SpriteName.SliderHorizontalForegroundButton,
					SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
				spyDefenseScrollbars[i] = new ScrollBar(x + 145, y + 85 + (150 * i), 16, 174, 1, 101, true, true, SpriteName.ScrollLeftBackgroundButton, SpriteName.ScrollLeftForegroundButton,
					SpriteName.ScrollRightBackgroundButton, SpriteName.ScrollRightForegroundButton, SpriteName.SliderHorizontalBackgroundButton, SpriteName.SliderHorizontalForegroundButton,
					SpriteName.SliderHorizontalBar, SpriteName.SliderHighlightedHorizontalBar);
			}
			empireScrollBar = new ScrollBar(x + 384, y, 16, 568, 4, 10, false, false, SpriteName.ScrollUpBackgroundButton, SpriteName.ScrollUpForegroundButton, SpriteName.ScrollDownBackgroundButton,
				SpriteName.ScrollDownForegroundButton, SpriteName.ScrollVerticalBackgroundButton, SpriteName.ScrollVerticalForegroundButton, SpriteName.ScrollVerticalBar, SpriteName.ScrollVerticalBar);

			incomingMessages = new Button[4];
			for (int i = 0; i < incomingMessages.Length; i++)
			{
				incomingMessages[i] = new Button(SpriteName.IncomingMessageBackground, SpriteName.IncomingMessageForeground, string.Empty, x + 98, y + 98 + (150 * i), 40, 40);
			}
			IncomingMessageTextBox = new Label(x + 405, y + 320);

			messageOptions = new Button[10];
			messageOptions[TRADE] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Trade", x + 405, y + 320, 190, 25);
			messageOptions[RESEARCH] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Research", x + 605, y + 320, 190, 25);
			messageOptions[ALLY] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Ally", x + 405, y + 350, 190, 25);
			messageOptions[UNALLY] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Unally", x + 605, y + 350, 190, 25);
			messageOptions[HARASS] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Harass Selected Empire", x + 405, y + 410, 190, 25);
			messageOptions[RECONCILE] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Reconcile Selected Empire", x + 605, y + 410, 190, 25);
			messageOptions[WAR] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "War", x + 505, y + 500, 190, 25);
			messageOptions[SEND] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Send Message", x + 505, y + 570, 190, 25);
			messageOptions[ACCEPT] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Accept", x + 405, y + 570, 190, 25);
			messageOptions[REJECT] = new Button(SpriteName.MiniBackgroundButton, SpriteName.MiniForegroundButton, "Decline", x + 605, y + 570, 190, 25);
			whichContactSelected = -1;
			isViewingReceivedMessage = false;

			spriteNames = new List<SpriteName>();
			spriteNames.Add(SpriteName.MiniBackgroundButton);
			spriteNames.Add(SpriteName.MiniForegroundButton);
			spriteNames.Add(SpriteName.ScrollUpBackgroundButton);
			spriteNames.Add(SpriteName.ScrollUpForegroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalBar);
			spriteNames.Add(SpriteName.ScrollDownBackgroundButton);
			spriteNames.Add(SpriteName.ScrollDownForegroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalBackgroundButton);
			spriteNames.Add(SpriteName.ScrollVerticalForegroundButton);
			whichMessageToSend = -1;*/

			reason = null;
			return true;
		}

		public void DrawScreen()
		{
			/*gameMain.DrawGalaxyBackground();
			drawingManagement.DrawSprite(SpriteName.ControlBackground, x, y, 255, 800, 600, System.Drawing.Color.White);
			for (int i = 0; i < maxVisible; i++)
			{
				empireButtons[i].Draw(drawingManagement);
				spyEffortScrollbars[i].DrawScrollBar(drawingManagement);
				spyDefenseScrollbars[i].DrawScrollBar(drawingManagement);
				drawingManagement.DrawSprite(SpriteName.RelationBar, x + 145, y + 124 + (150 * i), 255, System.Drawing.Color.White);
				drawingManagement.DrawSprite(SpriteName.RelationSlider, x + 135 + empiresInContact[i + empireScrollBar.TopIndex].RelationshipStatus, y + 122 + (150 * i), 255, System.Drawing.Color.White);
				avatars[i].Draw(x + 10, y + 10 + (150 * i));
				drawingManagement.DrawSprite(SpriteName.Spy, x + 145, y + 24 + (150 * i), 255, System.Drawing.Color.White);
				drawingManagement.DrawSprite(SpriteName.Security, x + 145, y + 64 + (150 * i), 255, System.Drawing.Color.White);
				drawingManagement.DrawSprite(SpriteName.Relation, x + 145, y + 104 + (150 * i), 255, System.Drawing.Color.White);
				relationLabels[i].Draw();
				empireNameLabels[i].Draw();
				if (empiresInContact[i + empireScrollBar.TopIndex].IncomingMessage != MessageType.NONE)
				{
					incomingMessages[i].Draw(drawingManagement);
				}
				if (empiresInContact[i + empireScrollBar.TopIndex].OutgoingMessage != MessageType.NONE)
				{
					drawingManagement.DrawSprite(SpriteName.OutgoingMessageBackground, x + 10, y + 98 + (150 * i), 255, System.Drawing.Color.White);
				}
			}
			empireScrollBar.DrawScrollBar(drawingManagement);
			if (whichContactSelected >= 0)
			{
				profile.Draw(x + 450, y + 10);
				if (!isViewingReceivedMessage)
				{
					for (int i = 0; i < 8; i++)
					{
						messageOptions[i].Draw(drawingManagement);
					}
					listOfEmpires.Draw(drawingManagement);
				}
				else
				{
					IncomingMessageTextBox.Draw();
					messageOptions[ACCEPT].Draw(drawingManagement);
					messageOptions[REJECT].Draw(drawingManagement);
				}
			}*/
		}

		public void Update(int x, int y, float frameDeltaTime)
		{
			/*if (empireScrollBar.UpdateHovering(x, y, frameDeltaTime))
			{
				RefreshContactList();
			}
			for (int i = 0; i < maxVisible; i++)
			{
				if (spyEffortScrollbars[i].UpdateHovering(x, y, frameDeltaTime))
				{
					empiresInContact[i + empireScrollBar.TopIndex].SpyEffort = spyEffortScrollbars[i].TopIndex;
					return;
				}
				if (spyDefenseScrollbars[i].UpdateHovering(x, y, frameDeltaTime))
				{
					empiresInContact[i + empireScrollBar.TopIndex].AntiSpyEffort = spyDefenseScrollbars[i].TopIndex;
					return;
				}
				if (empiresInContact[i + empireScrollBar.TopIndex].IncomingMessage != MessageType.NONE)
				{
					incomingMessages[i].UpdateHovering(x, y, frameDeltaTime);
				}
			}
			if (whichContactSelected >= 0)
			{
				if (!isViewingReceivedMessage)
				{
					for (int i = 0; i < 8; i++)
					{
						messageOptions[i].UpdateHovering(x, y, frameDeltaTime);
					}
					listOfEmpires.UpdateHovering(x, y, frameDeltaTime);
				}
				else
				{
					messageOptions[ACCEPT].UpdateHovering(x, y, frameDeltaTime);
					messageOptions[REJECT].UpdateHovering(x, y, frameDeltaTime);
				}
			}*/
		}

		public void MouseDown(int x, int y, int whichButton)
		{
			/*if (empireScrollBar.MouseDown(x, y))
			{
				return;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				if (spyEffortScrollbars[i].MouseDown(x, y))
				{
					return;
				}
				if (spyDefenseScrollbars[i].MouseDown(x, y))
				{
					return;
				}
				if (empiresInContact[i + empireScrollBar.TopIndex].IncomingMessage != MessageType.NONE)
				{
					if (incomingMessages[i].MouseDown(x, y))
					{
						return;
					}
				}
				if (empireButtons[i].MouseDown(x, y))
				{
					return;
				}
			}
			if (whichContactSelected >= 0)
			{
				if (!isViewingReceivedMessage)
				{
					if (listOfEmpires.MouseDown(x, y))
					{
						return;
					}
					for (int i = 0; i < 8; i++)
					{
						messageOptions[i].MouseDown(x, y);
					}
				}
				else
				{
					messageOptions[ACCEPT].MouseDown(x, y);
					messageOptions[REJECT].MouseDown(x, y);
				}
			}*/
		}

		public void MouseUp(int x, int y, int whichButton)
		{
			/*if (empireScrollBar.MouseUp(x, y))
			{
				RefreshContactList();
				return;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				if (spyEffortScrollbars[i].MouseUp(x, y))
				{
					empiresInContact[i + empireScrollBar.TopIndex].SpyEffort = spyEffortScrollbars[i].TopIndex;
					return;
				}
				if (spyDefenseScrollbars[i].MouseUp(x, y))
				{
					empiresInContact[i + empireScrollBar.TopIndex].AntiSpyEffort = spyDefenseScrollbars[i].TopIndex;
					return;
				}
				if (empiresInContact[i + empireScrollBar.TopIndex].IncomingMessage != MessageType.NONE)
				{
					if (incomingMessages[i].MouseUp(x, y))
					{
						whichContactSelected = i + empireScrollBar.TopIndex;
						foreach (Button button in empireButtons)
						{
							button.Selected = false;
						}
						empireButtons[i].Selected = true;
						LoadMessage();
						return;
					}
				}
				if (empireButtons[i].MouseUp(x, y))
				{
					whichContactSelected = i + empireScrollBar.TopIndex;
					foreach (Button button in empireButtons)
					{
						button.Selected = false;
					}
					empireButtons[i].Selected = true;
					LoadContact();
					return;
				}
			}
			if (whichContactSelected >= 0)
			{
				if (!isViewingReceivedMessage)
				{
					if (listOfEmpires.MouseUp(x, y))
					{
						return;
					}
					for (int i = 0; i < 8; i++)
					{
						if (messageOptions[i].MouseUp(x, y))
						{
							switch (i)
							{
								case TRADE:
								case RESEARCH:
								case ALLY:
								case UNALLY:
								case HARASS:
								case RECONCILE:
								case WAR:
									{
										for (int j = 0; j < 7; j++)
										{
											messageOptions[j].Selected = false;
										}
										messageOptions[i].Selected = true;
										whichMessageToSend = i;
									} break;
								case SEND:
									{
										switch (whichMessageToSend)
										{
											case TRADE:
												{
													if (empiresInContact[whichContactSelected].TradeTreaty)
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.BREAK_TRADE;
													}
													else
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.TRADE;
													}
												} break;
											case RESEARCH:
												{
													if (empiresInContact[whichContactSelected].ResearchTreaty)
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.BREAK_RESEARCH;
													}
													else
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.RESEARCH;
													}
												} break;
											case ALLY:
												{
													if (empiresInContact[whichContactSelected].NonAggression)
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.ALLIANCE;
													}
													else
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.NONAGGRESSION;
													}
												} break;
											case UNALLY:
												{
													if (empiresInContact[whichContactSelected].Allied)
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.BREAK_ALLIANCE;
													}
													else
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.BREAK_NONAGGRESSION;
													}
												} break;
											case HARASS:
												{
													empiresInContact[whichContactSelected].OutgoingMessage = MessageType.HARASS_EMPIRE;
													empiresInContact[whichContactSelected].OutgoingEmpireRequest = adjustedEmpiresForSelection[listOfEmpires.SelectedIndex];
												} break;
											case RECONCILE:
												{
													empiresInContact[whichContactSelected].OutgoingMessage = MessageType.RECONCILE_EMPIRE;
													empiresInContact[whichContactSelected].OutgoingEmpireRequest = adjustedEmpiresForSelection[listOfEmpires.SelectedIndex];
												} break;
											case WAR:
												{
													if (empiresInContact[whichContactSelected].AtWar)
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.OFFER_PEACE;
													}
													else
													{
														empiresInContact[whichContactSelected].OutgoingMessage = MessageType.WAR;
													}
												} break;
										}
										LoadContact();
										break;
									}
							}
						}
					}
				}
				else
				{
					if (messageOptions[ACCEPT].MouseUp(x, y))
					{
						switch (empiresInContact[whichContactSelected].IncomingMessage)
						{
							case MessageType.HARASS_EMPIRE:
								empiresInContact[whichContactSelected].OutgoingMessage = MessageType.ACCEPT_HARASS;
								empiresInContact[whichContactSelected].OutgoingEmpireRequest = empiresInContact[whichContactSelected].IncomingEmpireRequest;
								break;
							case MessageType.RECONCILE_EMPIRE:
								empiresInContact[whichContactSelected].OutgoingMessage = MessageType.ACCEPT_RECONCILE;
								empiresInContact[whichContactSelected].OutgoingEmpireRequest = empiresInContact[whichContactSelected].IncomingEmpireRequest;
								break;
							case MessageType.ALLIANCE:
								empiresInContact[whichContactSelected].OutgoingMessage = MessageType.ACCEPT_ALLIANCE;
								break;
							case MessageType.NONAGGRESSION:
								empiresInContact[whichContactSelected].OutgoingMessage = MessageType.ACCEPT_NONAGGRESSION;
								break;
							case MessageType.OFFER_PEACE:
								empiresInContact[whichContactSelected].OutgoingMessage = MessageType.ACCEPT_PEACE;
								break;
							case MessageType.RESEARCH:
								empiresInContact[whichContactSelected].OutgoingMessage = MessageType.ACCEPT_RESEARCH;
								break;
							case MessageType.TRADE:
								empiresInContact[whichContactSelected].OutgoingMessage = MessageType.ACCEPT_TRADE;
								break;
						}
						LoadMessage();
						return;
					}
					if (messageOptions[REJECT].MouseUp(x, y))
					{
						empiresInContact[whichContactSelected].OutgoingMessage = MessageType.DECLINE_REQUEST;
						LoadMessage();
						return;
					}
				}
			}*/
		}

		public void MouseScroll(int direction, int x, int y)
		{
		}

		public void SetupScreen()
		{
			/*empiresInContact = new List<Contact>();
			Empire currentEmpire = gameMain.EmpireManager.CurrentEmpire;
			foreach (Contact contact in currentEmpire.ContactManager.Contacts)
			{
				if (contact.Contacted)
				{
					empiresInContact.Add(contact);
				}
			}

			maxVisible = empiresInContact.Count > 4 ? 4 : empiresInContact.Count;
			avatars = new BBSprite[maxVisible];
			relationLabels = new Label[maxVisible];
			empireNameLabels = new Label[maxVisible];
			if (empiresInContact.Count > 4)
			{
				empireScrollBar.SetEnabledState(true);
				empireScrollBar.SetAmountOfItems(empiresInContact.Count);
			}
			else
			{
				empireScrollBar.SetEnabledState(false);
				empireScrollBar.SetAmountOfItems(10);
			}
			empireScrollBar.TopIndex = 0;
			whichMessageToSend = -1;
			whichContactSelected = -1;
			isViewingReceivedMessage = false;

			RefreshContactList();*/
		}

		/*private void RefreshContactList()
		{
			foreach (Button button in empireButtons)
			{
				button.Selected = false;
			}
			int whichButton = whichContactSelected - empireScrollBar.TopIndex;
			if (whichButton >= 0 && whichButton < 4)
			{
				empireButtons[whichButton].Selected = true;
			}
			for (int i = 0; i < maxVisible; i++)
			{
				spyEffortScrollbars[i].TopIndex = empiresInContact[i + empireScrollBar.TopIndex].SpyEffort;
				spyDefenseScrollbars[i].TopIndex = empiresInContact[i + empireScrollBar.TopIndex].AntiSpyEffort;
				avatars[i] = empiresInContact[i + empireScrollBar.TopIndex].EmpireInContact.EmpireRace.GetMiniAvatar();
				relationLabels[i] = new Label(Utility.RelationToLabel(empiresInContact[i + empireScrollBar.TopIndex].RelationshipStatus), x + 165, y + 104 + (i * 150));
				empireNameLabels[i] = new Label(empiresInContact[i + empireScrollBar.TopIndex].EmpireInContact.EmpireName, x + 145, y + 3 + (i * 150), empiresInContact[i + empireScrollBar.TopIndex].EmpireInContact.EmpireColor);
			}
		}

		private void LoadContact()
		{
			isViewingReceivedMessage = false;
			for (int i = 0; i < messageOptions.Length; i++)
			{
				messageOptions[i].Selected = false;
				messageOptions[i].Active = true;
			}
			Contact contact = empiresInContact[whichContactSelected];
			Expression whichExpression = Expression.NEUTRAL;
			if (contact.RelationshipStatus < 70)
			{
				whichExpression = Expression.ANGRY;
			}
			else if (contact.RelationshipStatus > 130)
			{
				whichExpression = Expression.HAPPY;
			}
			profile = contact.EmpireInContact.EmpireRace.GetAvatar(whichExpression);
			if (contact.AtWar)
			{
				messageOptions[TRADE].Active = false;
				messageOptions[RESEARCH].Active = false;
				messageOptions[ALLY].Active = false;
				messageOptions[UNALLY].Active = false;
				messageOptions[HARASS].Active = false;
				messageOptions[RECONCILE].Active = false;
				messageOptions[WAR].Active = true;
				messageOptions[WAR].SetText("Offer Peace");
			}
			else
			{
				messageOptions[TRADE].Active = true;
				messageOptions[RESEARCH].Active = true;
				messageOptions[ALLY].Active = true;
				messageOptions[UNALLY].Active = true;
				messageOptions[HARASS].Active = true;
				messageOptions[RECONCILE].Active = true;
				messageOptions[WAR].Active = true;
				messageOptions[WAR].SetText("Declare War");
			}
			if (contact.Allied)
			{
				messageOptions[ALLY].Active = false;
				messageOptions[ALLY].SetText("Already Allied");
				messageOptions[UNALLY].SetText("Break Alliance");
			}
			else if (contact.NonAggression)
			{
				messageOptions[ALLY].SetText("Offer Alliance");
				messageOptions[UNALLY].SetText("Break Non-Aggression");
			}
			else if (!contact.AtWar)
			{
				messageOptions[UNALLY].Active = false;
				messageOptions[ALLY].SetText("Offer Non-Aggression");
				messageOptions[UNALLY].SetText("No Military Pacts to Break");
			}
			if (contact.TradeTreaty)
			{
				messageOptions[TRADE].SetText("Halt Trade");
			}
			else
			{
				messageOptions[TRADE].SetText("Offer Trade");
			}
			if (contact.ResearchTreaty)
			{
				messageOptions[RESEARCH].SetText("Halt Shared Research");
			}
			else
			{
				messageOptions[RESEARCH].SetText("Offer Shared Research");
			}

			List<string> empires = new List<string>();
			adjustedEmpiresForSelection = new List<Empire>();
			foreach (Contact empireInContact in empiresInContact)
			{
				if (empireInContact != empiresInContact[whichContactSelected])
				{
					empires.Add(empireInContact.EmpireInContact.EmpireName);
					adjustedEmpiresForSelection.Add(empireInContact.EmpireInContact);
				}
			}

			listOfEmpires = new ComboBox(spriteNames, empires, x + 505, y + 440, 200, 25, 6);
			if (empires.Count == 0)
			{
				listOfEmpires.Active = false;
				messageOptions[RECONCILE].Active = false;
				messageOptions[HARASS].Active = false;
			}

			if (empiresInContact[whichContactSelected].OutgoingMessage != MessageType.NONE)
			{
				//already sending a message, disable everything
				for (int i = 0; i < 8; i++)
				{
					messageOptions[i].Active = false;
				}
				listOfEmpires.Active = false;
				switch (empiresInContact[whichContactSelected].OutgoingMessage)
				{
					case MessageType.BREAK_TRADE:
					case MessageType.TRADE: messageOptions[TRADE].Selected = true;
						break;
					case MessageType.BREAK_RESEARCH:
					case MessageType.RESEARCH: messageOptions[RESEARCH].Selected = true;
						break;
					case MessageType.ALLIANCE:
					case MessageType.NONAGGRESSION: messageOptions[ALLY].Selected = true;
						break;
					case MessageType.BREAK_NONAGGRESSION:
					case MessageType.BREAK_ALLIANCE: messageOptions[UNALLY].Selected = true;
						break;
					case MessageType.HARASS_EMPIRE: messageOptions[HARASS].Selected = true;
						for (int i = 0; i < adjustedEmpiresForSelection.Count; i++)
						{
							if (adjustedEmpiresForSelection[i] == empiresInContact[whichContactSelected].OutgoingEmpireRequest)
							{
								listOfEmpires.SelectedIndex = i;
								break;
							}
						}
						break;
					case MessageType.RECONCILE_EMPIRE: messageOptions[RECONCILE].Selected = true;
						for (int i = 0; i < adjustedEmpiresForSelection.Count; i++)
						{
							if (adjustedEmpiresForSelection[i] == empiresInContact[whichContactSelected].OutgoingEmpireRequest)
							{
								listOfEmpires.SelectedIndex = i;
								break;
							}
						}
						break;
					case MessageType.WAR:
					case MessageType.OFFER_PEACE: messageOptions[WAR].Selected = true;
						break;
				}
				messageOptions[7].Selected = true;
			}
		}
		private void LoadMessage()
		{
			isViewingReceivedMessage = true;
			Contact contact = empiresInContact[whichContactSelected];
			Expression whichExpression = Expression.NEUTRAL;
			if (contact.RelationshipStatus < 70)
			{
				whichExpression = Expression.ANGRY;
			}
			else if (contact.RelationshipStatus > 130)
			{
				whichExpression = Expression.HAPPY;
			}
			profile = contact.EmpireInContact.EmpireRace.GetAvatar(whichExpression);
			MessageType whichMessage = empiresInContact[whichContactSelected].IncomingMessage;
			switch (contact.IncomingMessage)
			{
				case MessageType.TRADE:
					IncomingMessageTextBox.SetText("We want trade agreement, do you agree?");
					break;
				case MessageType.BREAK_TRADE:
					IncomingMessageTextBox.SetText("We don't want your cheap items anymore!");
					break;
				case MessageType.RESEARCH:
					IncomingMessageTextBox.SetText("We would like us to share our research.");
					break;
				case MessageType.BREAK_RESEARCH:
					IncomingMessageTextBox.SetText("We don't want your lousy research!");
					break;
				case MessageType.NONAGGRESSION:
					IncomingMessageTextBox.SetText("We want non-aggression!");
					break;
				case MessageType.ALLIANCE:
					IncomingMessageTextBox.SetText("We want alliance!");
					break;
				case MessageType.BREAK_NONAGGRESSION:
					IncomingMessageTextBox.SetText("We want aggression!");
					break;
				case MessageType.BREAK_ALLIANCE:
					IncomingMessageTextBox.SetText("We want to break our alliance!");
					break;
				case MessageType.HARASS_EMPIRE:
					IncomingMessageTextBox.SetText("We want you to bully " + empiresInContact[whichContactSelected].IncomingEmpireRequest.EmpireName + "!");
					break;
				case MessageType.RECONCILE_EMPIRE:
					IncomingMessageTextBox.SetText("We want you to make peace with " + empiresInContact[whichContactSelected].IncomingEmpireRequest.EmpireName + "!");
					break;
				case MessageType.WAR:
					IncomingMessageTextBox.SetText("We're going to KILL you!");
					break;
				case MessageType.OFFER_PEACE:
					IncomingMessageTextBox.SetText("Please have mercy on us!");
					break;
				case MessageType.ACCEPT_ALLIANCE:
					IncomingMessageTextBox.SetText("We agree to ally with you!");
					break;
				case MessageType.ACCEPT_NONAGGRESSION:
					IncomingMessageTextBox.SetText("We agree to not be aggressive!");
					break;
				case MessageType.ACCEPT_PEACE:
					IncomingMessageTextBox.SetText("We will spare you for now!");
					break;
				case MessageType.ACCEPT_RECONCILE:
					IncomingMessageTextBox.SetText("We will make peace with " + "!");
					break;
				case MessageType.ACCEPT_HARASS:
					IncomingMessageTextBox.SetText("We will bully " + "!");
					break;
				case MessageType.ACCEPT_RESEARCH:
					IncomingMessageTextBox.SetText("We accept the deal to share research!");
					break;
				case MessageType.ACCEPT_TRADE:
					IncomingMessageTextBox.SetText("We accept the trade offer!");
					break;
				case MessageType.DECLINE_REQUEST:
					IncomingMessageTextBox.SetText("We decline your request.");
					break;
			}
			if (whichMessage == MessageType.BREAK_ALLIANCE || whichMessage == MessageType.BREAK_NONAGGRESSION || whichMessage == MessageType.BREAK_RESEARCH ||
				whichMessage == MessageType.BREAK_TRADE || whichMessage == MessageType.WAR || whichMessage == MessageType.DECLINE_REQUEST)
			{
				messageOptions[ACCEPT].SetText("Oh really?");
				messageOptions[ACCEPT].Active = false;
				messageOptions[REJECT].SetText(string.Empty);
				messageOptions[REJECT].Active = false;
			}
			else if (whichMessage == MessageType.ACCEPT_ALLIANCE || whichMessage == MessageType.ACCEPT_HARASS || whichMessage == MessageType.ACCEPT_NONAGGRESSION ||
				whichMessage == MessageType.ACCEPT_PEACE || whichMessage == MessageType.ACCEPT_RECONCILE || whichMessage == MessageType.ACCEPT_RESEARCH ||
				whichMessage == MessageType.ACCEPT_TRADE)
			{
				messageOptions[ACCEPT].SetText("Yayification!");
				messageOptions[ACCEPT].Active = false;
				messageOptions[REJECT].SetText(string.Empty);
				messageOptions[REJECT].Active = false;
			}
			else
			{
				messageOptions[ACCEPT].SetText("Accept Offer");
				messageOptions[ACCEPT].Active = true;
				messageOptions[REJECT].SetText("Reject Offer");
				messageOptions[REJECT].Active = true;
			}
			messageOptions[ACCEPT].Selected = false;
			messageOptions[REJECT].Selected = false;
			if (empiresInContact[whichContactSelected].OutgoingMessage != MessageType.NONE)
			{
				messageOptions[ACCEPT].Active = false;
				messageOptions[REJECT].Active = false;
				switch(empiresInContact[whichContactSelected].OutgoingMessage)
				{
					case MessageType.ACCEPT_ALLIANCE:
					case MessageType.ACCEPT_HARASS:
					case MessageType.ACCEPT_NONAGGRESSION:
					case MessageType.ACCEPT_PEACE:
					case MessageType.ACCEPT_RECONCILE:
					case MessageType.ACCEPT_RESEARCH:
					case MessageType.ACCEPT_TRADE:
						messageOptions[ACCEPT].Selected = true;
						break;
					case MessageType.DECLINE_REQUEST:
						messageOptions[REJECT].Selected = true;
						break;
				}
			}
		}*/

		public void KeyDown(KeyboardInputEventArgs e)
		{
			if (e.Key == KeyboardKeys.Escape)
			{
				gameMain.ChangeToScreen(Screen.Galaxy);
			}
		}
	}
}
