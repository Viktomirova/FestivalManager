using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Characters;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Items;

namespace WarCroft.Core
{
	public class WarController
	{
		private readonly IList<Character> characterParty;
		private readonly Stack<Item> itemPool;


		public WarController()
		{
			characterParty = new List<Character>();
			itemPool = new Stack<Item>();
		}

		public string JoinParty(string[] args)
		{
			if (args[0] == "Warrior")
			{
				characterParty.Add(new Warrior(args[1]));
			}
			else if (args[0] == "Priest")
			{
				characterParty.Add(new Priest(args[1]));
			}
			else
			{
				throw new ArgumentException(string.Format(ExceptionMessages.InvalidCharacterType, args[0]));
			}

			return string.Format(SuccessMessages.JoinParty, args[1]);
		}

		public string AddItemToPool(string[] args)
		{
			if (args[0] == "HealthPotion")
			{
				itemPool.Push(new HealthPotion());
			}
			else if (args[0] == "FirePotion")
			{
				itemPool.Push(new FirePotion());
			}
			else
			{
				throw new ArgumentException(string.Format(ExceptionMessages.InvalidItem, args[0]));
			}

			return string.Format(SuccessMessages.AddItemToPool, args[0]);
		}

		public string PickUpItem(string[] args)
		{
			Character character = characterParty.FirstOrDefault(n => n.Name == args[0]);
			if (character == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, args[0]));
			}

			if (itemPool.Count == 0)
			{
				throw new InvalidOperationException(ExceptionMessages.ItemPoolEmpty);
			}

			Item item = itemPool.Pop();
			character.Bag.AddItem(item);

			return string.Format(SuccessMessages.PickUpItem, character.Name, item.GetType().Name);
		}

		public string UseItem(string[] args)
		{
			Character character = characterParty.FirstOrDefault(n => n.Name == args[0]);
			if (character == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, args[0]));
			}

			Item item = character.Bag.GetItem(args[1]);
			character.UseItem(item);

			return string.Format(SuccessMessages.UsedItem, character.Name, item.GetType().Name);
		}

		public string GetStats()
		{
			StringBuilder sb = new StringBuilder();

			var characters = characterParty
			                 .OrderByDescending(c => c.IsAlive)
			                 .ThenByDescending(c => c.Health);

			foreach (var character in characters)
			{
				sb.AppendLine(string.Format(SuccessMessages.CharacterStats, character.Name, character.Health,
				                            character.BaseHealth, character.Armor, character.BaseArmor, character.IsAlive ? "Alive" : "Dead"));
			}

			return sb.ToString();
		}

		public string Attack(string[] args)
		{
			Character attacker = characterParty.FirstOrDefault(n => n.Name == args[0]);
			Character receiver = characterParty.FirstOrDefault(n => n.Name == args[1]);

			if (attacker == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, args[0]));
			}

			if (receiver == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, args[1]));
			}

			Warrior warrior = attacker as Warrior;

			if (warrior == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.AttackFail, args[0]));
			}

			warrior.Attack(receiver);

			string result = string.Format(SuccessMessages.AttackCharacter, warrior.Name, receiver.Name, warrior.AbilityPoints,
			                     receiver.Name, receiver.Health, receiver.BaseHealth, receiver.Armor,
			                     receiver.BaseArmor);
			if (receiver.Health == 0)
			{
				string status = string.Format(SuccessMessages.AttackKillsCharacter, receiver.Name);
				result = $"{result}\r\n{status}";
			}

			return result;
		}

		public string Heal(string[] args)
		{
			Character healer = characterParty.FirstOrDefault(n => n.Name == args[0]);
			Character healingReceiver = characterParty.FirstOrDefault(n => n.Name == args[1]);

			if (healer == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, args[0]));
			}

			if (healingReceiver == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, args[1]));
			}

			Priest priest = healer as Priest;

			if (priest == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.HealerCannotHeal, args[0]));
			}

			priest.Heal(healingReceiver);

			return string.Format(SuccessMessages.HealCharacter, priest.Name, healingReceiver.Name, priest.AbilityPoints,
			                     healingReceiver.Name, healingReceiver.Health);
		}
	}
}
