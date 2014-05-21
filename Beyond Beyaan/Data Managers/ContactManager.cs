using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class ContactManager
	{
		private List<Contact> contacts;
		private Empire thisEmpire;

		public List<Contact> Contacts
		{
			get { return contacts; }
		}

		public ContactManager(Empire currentEmpire, List<Empire> allEmpires)
		{
			thisEmpire = currentEmpire;
			contacts = new List<Contact>();
			/*foreach (Empire empire in allEmpires)
			{
				if (empire != currentEmpire)
				{
					Contact newContact = new Contact();
					newContact.EmpireInContact = empire;
					newContact.Contacted = true;
					newContact.RelationshipStatus = 100;
					newContact.OutgoingMessage = MessageType.NONE;
					newContact.IncomingMessage = MessageType.NONE;
					contacts.Add(newContact);
				}
			}*/
		}

		public void EstablishContact(Empire empireContacted, SitRepManager sitRepManager)
		{
			foreach (Contact contact in contacts)
			{
				if (contact.EmpireInContact == empireContacted && !contact.Contacted)
				{
					//Need to add some fudge factor
					contact.RelationshipStatus = (int)(thisEmpire.EmpireRace.CharismaMultipler * empireContacted.EmpireRace.CharismaMultipler * 500);
					contact.Contacted = true;
					contact.OutgoingMessage = MessageType.NONE;
					contact.IncomingMessage = MessageType.NONE;
					sitRepManager.AddItem(new SitRepItem(Screen.Diplomacy, null, null, new Point(-1, -1), "You have established contact with " + empireContacted.EmpireName + " empire."));
					return;
				}
			}
		}

		public void UpdateContacts(SitRepManager sitRepManager)
		{
			//Handle messages

			foreach (Contact contact in contacts)
			{
				if (!contact.Contacted)
				{
					continue;
				}
				Empire whichEmpireInRequest;
				contact.IncomingMessage = contact.EmpireInContact.ContactManager.GetMessage(thisEmpire, out whichEmpireInRequest);
				contact.IncomingEmpireRequest = whichEmpireInRequest;
				if (contact.IncomingMessage != MessageType.NONE)
				{
					sitRepManager.AddItem(new SitRepItem(Screen.Diplomacy, null, null, new Point(), "You have received a message from the " + contact.EmpireInContact.EmpireName + " Empire."));
					HandleMessage(contact, contact.IncomingMessage, contact.IncomingEmpireRequest);
				}
			}

			//Handle relationship changes
		}

		private MessageType GetMessage(Empire forWhichEmpire, out Empire whichEmpireInRequest)
		{
			foreach (Contact contact in contacts)
			{
				if (contact.EmpireInContact == forWhichEmpire && contact.OutgoingMessage != MessageType.NONE)
				{
					whichEmpireInRequest = contact.OutgoingEmpireRequest;
					HandleMessage(contact, contact.OutgoingMessage, whichEmpireInRequest);
					MessageType message = contact.OutgoingMessage;
					//clear the outgoing message since it's now processed
					contact.OutgoingMessage = MessageType.NONE;
					contact.OutgoingEmpireRequest = null;
					return message;
				}
			}
			whichEmpireInRequest = null;
			return MessageType.NONE;
		}

		public bool IsContacted(Empire empire)
		{
			foreach (Contact contact in contacts)
			{
				if (contact.EmpireInContact == empire)
				{
					return contact.Contacted;
				}
			}
			return false;
		}

		private void UpdateRelationship(Empire currentEmpire, Contact whichContact, int amount)
		{

		}

		private void HandleMessage(Contact contact, MessageType message, Empire whichEmpire)
		{
			switch (message)
			{
				case MessageType.ACCEPT_ALLIANCE:
					{
						contact.Allied = true;
						contact.ModifyRelationship(20);
					} break;
				case MessageType.ACCEPT_NONAGGRESSION:
					{
						contact.NonAggression = true;
						contact.ModifyRelationship(10);
					} break;
				case MessageType.ACCEPT_TRADE:
					{
						contact.TradeTreaty = true;
						contact.TradePercentage = -5;
						contact.ModifyRelationship(5);
					} break;
				case MessageType.ACCEPT_RESEARCH:
					{
						contact.ResearchTreaty = true;
						contact.ResearchPercentage = -5;
						contact.ModifyRelationship(5);
					} break;
				case MessageType.BREAK_ALLIANCE:
					{
						contact.Allied = false;
						contact.ModifyRelationship(-30);
					} break;
				case MessageType.BREAK_NONAGGRESSION:
					{
						contact.NonAggression = false;
						contact.ModifyRelationship(-20);
					} break;
				case MessageType.BREAK_RESEARCH:
					{
						contact.ResearchTreaty = false;
						contact.ResearchPercentage = 0;
						contact.ModifyRelationship(-10);
					} break;
				case MessageType.BREAK_TRADE:
					{
						contact.TradeTreaty = false;
						contact.TradePercentage = 0;
						contact.ModifyRelationship(-10);
					} break;
				case MessageType.DECLINE_REQUEST:
					{
						contact.ModifyRelationship(-5);
					} break;
				case MessageType.WAR:
					{
						contact.AtWar = true;
						contact.ModifyRelationship(-75);
					} break;
				case MessageType.ACCEPT_PEACE:
					{
						contact.AtWar = false;
						contact.ModifyRelationship(15);
					} break;
				case MessageType.ACCEPT_RECONCILE:
					{
						foreach (Contact whichContact in contact.EmpireInContact.ContactManager.Contacts)
						{
							if (whichContact.EmpireInContact == whichEmpire)
							{
								whichContact.ModifyRelationship(10);
								return;
							}
						}
					} break;
				case MessageType.ACCEPT_HARASS:
					{
						foreach (Contact whichContact in contact.EmpireInContact.ContactManager.Contacts)
						{
							if (whichContact.EmpireInContact == whichEmpire)
							{
								whichContact.ModifyRelationship(-15);
								return;
							}
						}
					} break;
			}
		}
	}
}
