using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Modules
{
	public enum MessageType
	{
		ALLIANCE, NONAGGRESSION, TRADE, RESEARCH, OFFER_PEACE, WAR, BREAK_ALLIANCE, BREAK_NONAGGRESSION, BREAK_TRADE, BREAK_RESEARCH, HARASS_EMPIRE, RECONCILE_EMPIRE, NONE,
						ACCEPT_ALLIANCE, ACCEPT_NONAGGRESSION, ACCEPT_TRADE, ACCEPT_HARASS, ACCEPT_RECONCILE, ACCEPT_RESEARCH, ACCEPT_PEACE, DECLINE_REQUEST}
	public class Contact
	{
		public Empire EmpireInContact { get; set; }
		public bool Contacted { get; set; }
		public bool TradeTreaty { get; set; }
		public bool ResearchTreaty { get; set; }
		public bool NonAggression { get; set; }
		public bool Allied { get; set; }
		public bool AtWar { get; set; }
		public int RelationshipStatus { get; set; }
		public int SpyEffort { get; set; }
		public int AntiSpyEffort { get; set; }
		public MessageType OutgoingMessage { get; set; }
		public MessageType IncomingMessage { get; set; }
		public Empire OutgoingEmpireRequest { get; set; } //Which empire to harass/make peace?
		public Empire IncomingEmpireRequest { get; set; }
		public int TradePercentage { get; set; } //Starts at -5%, then goes up to %15, with %1 per turn
		public int ResearchPercentage { get; set; } //Starts at -5%, then goes up to 15% with %1 per turn

		public void ModifyRelationship(int amount)
		{
			RelationshipStatus += amount;
			if (RelationshipStatus > 100)
			{
				RelationshipStatus = 100;
			}
			else if (RelationshipStatus < -100)
			{
				RelationshipStatus = -100;
			}
		}
	}
}
