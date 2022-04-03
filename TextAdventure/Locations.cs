using System;
using System.Collections.Generic;

namespace TextAdventure {

    class Locations {
        public enum Location {
            Invalid,
            Forest,
            Garden,
            Atrium,
            Kitchen,
            StaircaseL,
            StaircaseU,
            HallwayU,
            MasterBedroom,
            WaitingRoom,
            Study,
            Library,
            Larder,
            ServantsHall,
            Lab,
            Closet,
            BellTower
        }

        private static Dictionary<Location, int> mNumVisits;

        public static void Init() {
            mNumVisits = new Dictionary<Location, int>();
            for (int i = 1; i < Enum.GetNames(typeof(Location)).Length; i++) {
                mNumVisits.Add((Location)i, 0);
            }

            mNumVisits[Location.Forest] = 1;
        }

        private static void TryMove(Action action, Location north, Location south, Location east, Location west,
                                                   Location up = Location.Invalid, Location down = Location.Invalid) {

            bool bNorth = (north != Location.Invalid);
            bool bSouth = (south != Location.Invalid);
            bool bEast = (east != Location.Invalid);
            bool bWest = (west != Location.Invalid);

            bool bUp = (up != Location.Invalid);
            bool bDown = (down != Location.Invalid);

            switch (action.type) {
                case Action.ActionType.North:
                    if (bNorth) {
                        Program.SetPlayerLocation(north);
                        Program.SetMessage("You went north.");
                        mNumVisits[north]++;
                    } else Program.SetMessage("You can't go that way.");
                    break;

                case Action.ActionType.South:
                    if (bSouth) {
                        Program.SetPlayerLocation(south);
                        Program.SetMessage("You went south.");
                        mNumVisits[south]++;
                    } else Program.SetMessage("You can't go that way.");
                    break;

                case Action.ActionType.East:
                    if (bEast) {
                        Program.SetPlayerLocation(east);
                        Program.SetMessage("You went east.");
                        mNumVisits[east]++;
                    } else Program.SetMessage("You can't go that way.");
                    break;

                case Action.ActionType.West:
                    if (bWest) {
                        Program.SetPlayerLocation(west);
                        Program.SetMessage("You went west.");
                        mNumVisits[west]++;
                    } else Program.SetMessage("You can't go that way.");
                    break;

                case Action.ActionType.Up:
                    if (bUp) {
                        Program.SetPlayerLocation(up);
                        Program.SetMessage("You went up.");
                        mNumVisits[up]++;
                    } else Program.SetMessage("You can't go that way.");
                    break;

                case Action.ActionType.Down:
                    if (bDown) {
                        Program.SetPlayerLocation(down);
                        Program.SetMessage("You went down.");
                        mNumVisits[down]++;
                    } else Program.SetMessage("You can't go that way.");
                    break;
            }

            Renderer.DrawCompass(bNorth, bSouth, bEast, bWest);
        }

        private static void Forest(Action action) {
            string name = "Forest";
            string description =
                @"You find yourself in the clearing of a
              dense forest. The gnarly roots of
              decaying trees erupt from the ground,
              snagging your feet like tentacles. A
              fresh parting in the bushes lies to
              your {03 east}.";

            string specialInfo =
                @"You are walking through a forest with your friend. As you press on through the densely packed trees, an ear piercing howl followed by a sharp
              scream fills the air. You whip around only to find that your friend is gone, the only hint of their whereabouts a fresh parting in the bushes
              to your {03 east}. You feel a sharp sting in the back of your neck and realize that you are bleeding. A numbness is spreading around your body, as 
              if you have been {13 poisoned}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            if (mNumVisits[Location.Forest] == 1) Renderer.DrawSpecialInfo(specialInfo);

            TryMove(action, Location.Invalid, Location.Invalid, Location.Garden, Location.Invalid);

        }

        private static void Garden(Action action) {
            string name = "Garden";
            string description =
                @"You find yourself in a small garden;
              cracked gravestones line the way {03 north},
              leading up to the doors of a grand
              dilapidated mansion. The garden is 
              surrounded by thick bushes baring
              strange {13 berries}. To your {03 west} is a small
              parting in the bushes.";

            string specialInfo =
                @"As you emerge from the bushes, your view is filled by a mansion of incomprehensible scale. You clutch your head as you feel the {13 poison} filling
              your body.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            if (mNumVisits[Location.Garden] == 1) {
                Renderer.DrawSpecialInfo(specialInfo);
            }

            if (action.type == Action.ActionType.Take && action.arg == "berries") {
                Program.GivePlayerItems("berries", 2);
                Program.SetMessage("You take some of the strange berries.");
            }

            TryMove(action, Location.Atrium, Location.Invalid, Location.Invalid, Location.Forest);
        }

        private static void Atrium(Action action) {
            string name = "Atrium";
            string description =
                @"You walk into a vast hallway. Fraying 
            red carpet leads {03 north} from the main
            entrance towards a spiral staircase.
            Faded paintings cover the walls, and
            cobwebs hang from the rotting ornamental
            roof above you. There are doors to your
            {03 east} and {03 west}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            if (Program.GetPlayerInfo().inverntory["master key"] != 0) {
                TryMove(action, Location.StaircaseL, Location.Garden, Location.WaitingRoom, Location.Kitchen);
            } else {
                TryMove(action, Location.StaircaseL, Location.Garden, Location.WaitingRoom, Location.Invalid);
                Renderer.DrawCompass(true, true, true, true);
                if (action.type == Action.ActionType.West) {
                    Program.SetMessage("You try to open the door but it is locked.");
                }
            }

        }

        private static void WaitingRoom(Action action) {
            string name = "Waiting Room";
            string description =
                @"You walk into a waiting room. The wall
            to your east is filled with shelves upon
            shelves of glass tanks containing moth
            eaten taxidermy animals. As you walk,
            their glass eyes appear to follow you.
            There are doors to your {03 west} and {03 north}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            TryMove(action, Location.Study, Location.Invalid, Location.Invalid, Location.Atrium);
        }

        private static void Study(Action action) {
            string name = "Study";
            string description1 =
                @"You walk into a large study. An
            oak desk fills the room, sagging under
            the wait of countless books and papers.
            The wall to the west if covered by an
            enormous painting, although the canvas
            has been brutally slashed, making it 
            impossible to make out the figure. To 
            your {03 east} is a vast library, and to
            your {03 south} is a door.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description1);

            if (action.type == Action.ActionType.Search && action.arg == "painting") {
                Program.SetMessage("Behind the painting you found a staircase which you can go down.");
            }

            TryMove(action, Location.Invalid, Location.WaitingRoom, Location.Library, Location.Invalid, Location.Invalid, Location.Lab);

        }

        private static void Library(Action action) {
            string name = "Library";
            string description =
                @"You are in a vast library. To
            the {03 east}, creaking shelves stretch as
            far as you can see, each filled with
            countless yellowing books. You can exit
            to the {03 west}";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            if (action.type == Action.ActionType.Take && action.arg == "book") {
                Program.SetMessage("You took a book");
                Program.GivePlayerItems("book", 1);
            }

            TryMove(action, Location.Invalid, Location.Invalid, Location.Library, Location.Study);

        }

        private static void StaircaseL(Action action) {
            string name = "Staircase (Lower)";
            string description =
                @"You stand at the bottom of a grand
              spiral staircase. The wooden stairs
              audibly creak under foot, but the 
              railings are so rusted and warped that
              they provide no safety. The staircase
              continues { 03 up}ward, and you can exit
              the stairwell to the { 03 south}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            TryMove(action, Location.Invalid, Location.Atrium, Location.Invalid, Location.Invalid, Location.StaircaseU);

        }

        private static void StaircaseU(Action action) {
            string name = "Staircase (Upper)";
            string description =
                @"You are now on the first floor. You 
              stand next to a grand spiral staircase.
              The wooden stairs audibly creak under
              foot, but the railings are so rusted and
              warped that they provide no safety. The
              staircase once continued {03 up} another
              floor, however it has since crumbled and
              collapsed. There is a door to your {03 south}
              and you can go back {03 down} the stairs.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            if (action.type == Action.ActionType.Use && action.arg == "potion") {
                Program.SetPlayerLocation(Location.BellTower);
            }

            TryMove(action, Location.Invalid, Location.HallwayU, Location.Invalid, Location.Invalid, Location.Invalid, Location.StaircaseL);

        }

        private static void HallwayU(Action action) {
            string name = "Hallway (Upper)";
            string description =
                @"You walk into a hallway, brushing from
              your face the spiders webs which hang
              thickly from the ceiling. There are
              doors to your {03 north} and {03 west}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            TryMove(action, Location.StaircaseU, Location.Invalid, Location.Invalid, Location.MasterBedroom);

        }

        private static void MasterBedroom(Action action) {
            string name = "Master Bedroom";
            string description =
                @"You find yourself in a large bedroom.
              The once ornate wallpaper hanging from
              the walls is now faded and tattered,
              as is are the curtains surrounding the
              four poster bed in the center of the
              room. In the {14 corner}, you notice a
              glint of metal. There is a door to your
              {03 east}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);


            if (action.type == Action.ActionType.Search && action.arg == "corner") {
                Program.SetMessage("You examine the corner closer and find a key rusted to the floor.");
            }
            if (action.type == Action.ActionType.Take && action.arg == "key" && Program.GetPlayerInfo().inverntory["master key"] == 0) {
                Program.GivePlayerItems("master key", 1);
                Program.SetMessage("You took the key");
            }

            TryMove(action, Location.Invalid, Location.Invalid, Location.HallwayU, Location.Invalid);

        }

        private static void Kitchen(Action action) {
            string name = "Kitchen";
            string description =
                @"You walk into a kitchen and are
              greeted by the overpowering stench
              of rotting flesh. Fresh blood is 
              splattered on the walls, and rusty
              knives lay discarded on floor. There
              are doors to your {03 north} {03 east}
              and {03 west}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            TryMove(action, Location.Larder, Location.Invalid, Location.Atrium, Location.ServantsHall);

        }

        private static void Larder(Action action) {
            string name = "Larder";
            string description =
                @"You find yourself in a small larder.
              The smell is unbearable now, owing to
              the racks of fly covered rotting meat
              which line the walls. You can exit to 
              the {03 south}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            TryMove(action, Location.Invalid, Location.Kitchen, Location.Invalid, Location.Invalid);

        }

        private static void ServantsHall(Action action) {
            string name = "Servants Hall";
            string description =
                @"You walk into a small, lightly
              furnished room. A wooden table and
              chairs sit in one corner. The 
              opposite side of the room has been
              completely swallowed up by a gaping
              chasm, which even light cannot seem
              to escape. The wooden floor creaks
              underfoot, threatening to give way and
              tumble into the pit below. There is
              a door to your {03 east}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            string specialInfo1 =
                @"As you walk in, a rasping voice whispers into your ear. 'So, you've come after your friend, have you?' it sneers. 'Well if you want to see them
              again, you'll have to {14 give} me something in return.' I require a {14 book}. It gets awfully boring in here, you know.' The creature lets out a
              manic laugh.";

            string specialInfo2 =
                @"'Well? Have you got it?' Says the creature desperately.";

            string specialInfo3 =
                @"'Thank you' says the voice, letting out a manic laugh. Now for my end of the bargain. What you seeks lays beyond an enormous {14 painting}.            ";

            if (action.type == Action.ActionType.Give && action.arg == "book" && !Program.GetPlayerInfo().givenBook) {
                Program.SetPlayerHasGivenBook();
                Program.SetMessage("You give the book to the creature");
            }

            if (Program.GetPlayerInfo().givenBook) {
                Renderer.DrawSpecialInfo(specialInfo3);
            } else {
                if (mNumVisits[Location.ServantsHall] == 1) {
                    Renderer.DrawSpecialInfo(specialInfo1);
                } else {
                    Renderer.DrawSpecialInfo(specialInfo2);
                }
            }

            TryMove(action, Location.Invalid, Location.Invalid, Location.Kitchen, Location.Invalid);

        }

        private static void Lab(Action action) {
            string name = "Laboratory";
            string description =
                @"You find yourself in a large concrete
              room. Stained wooden tables litter the
              floor, covered by papers, broken test 
              tubes and spilt chemicals. At one desk
              you see several {13 potion}s. There
              are stairs to your {03 west} and a door 
              to your {03 south}";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            if (action.type == Action.ActionType.Take && action.arg == "potion") {
                Program.GivePlayerItems("potion", 2);
                Program.SetMessage("You pick up some of the potions");
            }

            TryMove(action, Location.Invalid, Location.Closet, Location.Invalid, Location.StaircaseL);

        }

        private static void Closet(Action action) {
            string name = "Closet";
            string description =
                @"You walk into a small closet. Skeletons
                  and fragments of bone are piled up on
                  the floor. There is a door to your
                  {03 north}.";

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);

            if (action.type == Action.ActionType.Take && action.arg == "potion") {
                Program.GivePlayerItems("potion", 2);
                Program.SetMessage("You pick up some of the potions");
            }

            TryMove(action, Location.Lab, Location.Invalid, Location.Invalid, Location.Invalid);

        }

        private static void BellTower(Action action) {
            string name = "Bell Tower";
            string description =
                @"You find yourself in a cramped bell
                  tower. You hear muffled screaming
                  from behind some {14 boxes}.";

            string speicalInfo = "You drink the potion and begin to feel weightless. You float up, past the broken stairs into a cramped bell tower.";

            Program.SetMessage("");

            Renderer.DrawLocationName(name);
            Renderer.DrawLocationInfo(description);
            Renderer.DrawSpecialInfo(speicalInfo);

            if (action.type == Action.ActionType.Search && action.arg == "boxes") {
                Program.SetGameState(GameState.End);
            }

            TryMove(action, Location.Invalid, Location.Invalid, Location.Invalid, Location.Invalid);

        }

        public static void SelectLocation(Location location, Action action) {
            switch (location) {
                case Location.Forest:
                    Forest(action);
                    break;

                case Location.Garden:
                    Garden(action);
                    break;

                case Location.Atrium:
                    Atrium(action);
                    break;

                case Location.WaitingRoom:
                    WaitingRoom(action);
                    break;

                case Location.Study:
                    Study(action);
                    break;

                case Location.Library:
                    Library(action);
                    break;

                case Location.StaircaseL:
                    StaircaseL(action);
                    break;

                case Location.StaircaseU:
                    StaircaseU(action);
                    break;

                case Location.HallwayU:
                    HallwayU(action);
                    break;

                case Location.MasterBedroom:
                    MasterBedroom(action);
                    break;

                case Location.Kitchen:
                    Kitchen(action);
                    break;

                case Location.Larder:
                    Larder(action);
                    break;

                case Location.ServantsHall:
                    ServantsHall(action);
                    break;

                case Location.Lab:
                    Lab(action);
                    break;

                case Location.Closet:
                    Closet(action);
                    break;

                case Location.BellTower:
                    BellTower(action);
                    break;

            }
        }
    }

}
