using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Data_Modules
{
	public enum Expression { HAPPY, NEUTRAL, ANGRY }
	public class RaceShipType
	{
		public string TypeName { get; set; }
		public int Space { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public List<BBSprite> Bodies { get; set; }
	}
	public class Race
	{
		public float AgricultureMultipler { get; private set; }
		public float WasteMultipler { get; private set; }
		public float CommerceMultipler { get; private set; }
		public float ResearchMultipler { get; private set; }
		public float ConstructionMultipler { get; private set; }

		public float EngineMultipler { get; private set; }
		public float ArmorMultipler { get; private set; }
		public float ShieldMultipler { get; private set; }
		public float BeamMultipler { get; private set; }
		public float ParticleMultipler { get; private set; }
		public float MissileMultipler { get; private set; }
		public float TorpedoMultipler { get; private set; }
		public float BombMultipler { get; private set; }

		public float GrowthMultipler { get; private set; }
		public float SpyMultipler { get; private set; }
		public float SpyDefenseMultipler { get; private set; }
		public float AccuracyMultipler { get; private set; }
		public float CharismaMultipler { get; private set; }

		public string RaceName { get; private set; }
		public string SingularRaceName { get; private set; }
		public string RaceDescription { get; private set; }

		public BBSprite NeutralAvatar { get; private set; }
		public BBSprite AngryAvatar { get; private set; }
		public BBSprite HappyAvatar { get; private set; }
		public BBSprite MiniAvatar { get; private set; }
		public List<RaceShipType> ShipTypes { get; private set; }
		public BBSprite GroundTroop { get; private set; }
		public BBSprite DyingTroop { get; private set; }
		public BBSprite FleetIcon { get; private set; }
		public BBSprite TransportIcon { get; private set; }
		public BBSprite City { get; private set; }
		public BBSprite LandingShip { get; private set; }

		public bool Initialize(FileInfo file, Random r, out string reason)
		{
			XDocument doc = XDocument.Load(file.FullName);
			XElement root = doc.Element("Race");

			SetBaseDefaults();

			RaceName = root.Attribute("name").Value;
			SingularRaceName = root.Attribute("singularName").Value;
			RaceDescription = root.Attribute("raceDescription").Value;
			NeutralAvatar = SpriteManager.GetSprite(root.Attribute("neutralPortrait").Value, r);
			HappyAvatar = SpriteManager.GetSprite(root.Attribute("happyPortrait").Value, r);
			AngryAvatar = SpriteManager.GetSprite(root.Attribute("angryPortrait").Value, r);
			MiniAvatar = SpriteManager.GetSprite(root.Attribute("thumbnail").Value, r);
			GroundTroop = SpriteManager.GetSprite(root.Attribute("groundTroop").Value, r);
			DyingTroop = SpriteManager.GetSprite(root.Attribute("dyingTroop").Value, r);
			FleetIcon = SpriteManager.GetSprite(root.Attribute("fleetIcon").Value, r);
			TransportIcon = SpriteManager.GetSprite(root.Attribute("transportIcon").Value, r);
			City = SpriteManager.GetSprite(root.Attribute("city").Value, r);
			LandingShip = SpriteManager.GetSprite(root.Attribute("landingShip").Value, r);
			
			XElement shipTypes = root.Element("ShipTypes");
			if (shipTypes == null)
			{
				reason = "ShipTypes not found in " + RaceName + " race";
				return false;
			}

			ShipTypes = new List<RaceShipType>();
			foreach (XElement shipType in shipTypes.Elements())
			{
				RaceShipType newType = new RaceShipType();
				newType.TypeName = shipType.Attribute("name").Value;
				newType.Space = int.Parse(shipType.Attribute("space").Value);
				newType.Width = int.Parse(shipType.Attribute("width").Value);
				newType.Height = int.Parse(shipType.Attribute("height").Value);
				newType.Bodies = new List<BBSprite>();
				foreach (XElement body in shipType.Elements())
				{
					newType.Bodies.Add(SpriteManager.GetSprite(body.Attribute("sprite").Value, r));
				}
				ShipTypes.Add(newType);
			}
			//Order ships based on space
			ShipTypes.Sort((a, b) => { return a.Space.CompareTo(b.Space); });

			reason = null;
			return true;
		}

		private void SetBaseDefaults()
		{
			AgricultureMultipler = 1.0f;
			WasteMultipler = 1.0f;
			CommerceMultipler = 1.0f;
			ResearchMultipler = 1.0f;
			ConstructionMultipler = 1.0f;
			EngineMultipler = 1.0f;
			ArmorMultipler = 1.0f;
			ShieldMultipler = 1.0f;
			BeamMultipler = 1.0f;
			ParticleMultipler = 1.0f;
			MissileMultipler = 1.0f;
			TorpedoMultipler = 1.0f;
			BombMultipler = 1.0f;
			GrowthMultipler = 1.0f;
			SpyMultipler = 1.0f;
			SpyDefenseMultipler = 1.0f;
			AccuracyMultipler = 1.0f;
			CharismaMultipler = 1.0f;
		}

		public string GetRandomShipName()
		{
			// TODO: Add ship names to races
			return NameGenerator.GetName();
		}

		public BBSprite GetShip(int type, int whichBody)
		{
			if (type < ShipTypes.Count)
			{
				if (whichBody < ShipTypes[type].Bodies.Count)
				{
					return ShipTypes[type].Bodies[whichBody];
				}
			}
			return null;
		}

		public BBSprite GetMiniAvatar()
		{
			return MiniAvatar;
		}

		public BBSprite GetAvatar(Expression whichExpression)
		{
			switch (whichExpression)
			{
				case Expression.ANGRY:
					return AngryAvatar;
				case Expression.HAPPY:
					return HappyAvatar;
			}
			return NeutralAvatar;
		}

		public string GetRandomEmperorName()
		{
			// TODO: Add potential emperor names
			return NameGenerator.GetName();
		}
	}
}
