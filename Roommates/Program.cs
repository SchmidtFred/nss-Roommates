using System;
using System.Collections.Generic;
using System.Linq;
using Roommates.Repositories;
using Roommates.Models;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        while (true)
                        {
                        Console.Write("Room Id: ");
                            try
                            {
                                int id = int.Parse(Console.ReadLine());

                                Room room = roomRepo.GetById(id);
                                if (room == null)
                                {
                                    Console.WriteLine("Room does not exist");
                                    continue;
                                }

                                Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                                Console.Write("Press any key to continue");
                                Console.ReadKey();
                                break;
                            } catch(Exception)
                            {
                                Console.WriteLine("Invalid option");
                                continue;
                            }
                        }
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        int max;

                        while (true)
                        {
                            try
                            {
                                Console.Write("Max occupancy: ");
                                max = int.Parse(Console.ReadLine());
                                break;
                            } catch (Exception)
                            {
                                continue;
                            }
                        }

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} MaxOccupancy({r.MaxOccupancy}");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(roomOptions => roomOptions.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} MaxOccupancy({r.MaxOccupancy}");
                        }

                        Console.Write("Which room would you like to delete? ");
                        selectedRoomId = int.Parse(Console.ReadLine());
                        selectedRoom = roomOptions.FirstOrDefault(roomOptions => roomOptions.Id == selectedRoomId);
                        roomRepo.Delete(selectedRoom.Id);
                        Console.Write($"{selectedRoom.Id} - {selectedRoom.Name} has been deleted");
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        while (true)
                        {
                            try
                            {
                                Console.Write("Chore Id: ");
                                int id = int.Parse(Console.ReadLine());

                                Chore chore = choreRepo.GetById(id);
                                if (chore == null)
                                {
                                    Console.WriteLine("Chore does not exist");
                                    continue;
                                }

                                Console.WriteLine($"{chore.Id} - {chore.Name}");
                                Console.Write("Press any key to continue");
                                Console.ReadKey();
                                break;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Invalid option");
                                continue;
                            }
                        }
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(choreOption => choreOption.Id == selectedChoreId);

                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine();

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a chore"):
                        choreOptions = choreRepo.GetAll();
                        foreach (Chore c in choreOptions)
                        {
                            Console.WriteLine($"{c.Id} - {c.Name}");
                        }

                        Console.Write("Which chore would you like to delete? ");
                        selectedChoreId = int.Parse(Console.ReadLine());
                        selectedChore = choreOptions.FirstOrDefault(choreOption => choreOption.Id == selectedChoreId);
                        choreRepo.Delete(selectedChore.Id);
                        Console.WriteLine($"{selectedChore.Id} - {selectedChore.Name} has been deleted");
                        break;
                    case ("Search for roommate"):
                        while (true)
                        {
                            try
                            {
                                Console.Write("Roommate Id: ");
                                int id = int.Parse(Console.ReadLine());

                                Roommate roommate = roommateRepo.GetById(id);
                                if (roommate == null)
                                {
                                    Console.WriteLine("Roommate does not exist");
                                    continue;
                                }
                                Console.WriteLine($"{roommate.FirstName} - Rent Portion: {roommate.RentPortion}% - Room: {roommate.Room.Name}");
                                Console.Write("Press any key to continue");
                                Console.ReadKey();
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                continue;
                            }
                        }
                        break;
                    case ("Show all unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Assign chore to roommate"):
                        int choreId, roommateId;
                        Chore chosenChore;
                        Roommate chosenRoommate;
                        //show chores
                        unassignedChores = choreRepo.GetUnassignedChores();
                        for (int i = 0; i < unassignedChores.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}) {unassignedChores[i].Name}");
                        }    
                        //choose chore
                        while (true)
                        {
                            try
                            {
                                Console.WriteLine();
                                Console.Write("Choose a chore: ");
                                int index = int.Parse(Console.ReadLine()) - 1;
                                chosenChore = unassignedChores[index];
                                choreId = chosenChore.Id;
                                break;
                            } catch (Exception)
                            {
                                Console.WriteLine("Invalid Option");
                                continue;
                            }
                        }
                        //show roommates
                        List<Roommate> roommates = roommateRepo.GetAll();
                        for (int i = 0; i < roommates.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}) {roommates[i].FirstName} {roommates[i].LastName}");
                        }
                        //choose roommate
                        while (true)
                        {
                            try
                            {
                                Console.WriteLine();
                                Console.Write("Choose a roommate: ");
                                int index = int.Parse(Console.ReadLine()) - 1;
                                chosenRoommate = roommates[index];
                                roommateId = chosenRoommate.Id;
                                break;
                            } catch (Exception)
                            {
                                Console.WriteLine("Invalid Option");
                                continue;
                            }
                        }
                        //create RoommateChore obect using ChoreRepository
                        choreRepo.AssignChore(roommateId, choreId);
                        Console.WriteLine($"{chosenRoommate.FirstName} has been assigned to {chosenChore.Name}");
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Update a chore",
                "Delete a chore",
                "Show all unassigned chores",
                "Search for roommate",
                "Assign chore to roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}