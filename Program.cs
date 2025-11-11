using System;

namespace NotesApp
{
    // VIEW - User interface
    class Program
    {
        static void Main()
        {
            var viewModel = new NotesViewModel();

            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║   MVVM Notes App - C# Console     ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            while (true)
            {
                Console.WriteLine("\n┌─ MENU ─────────────────────────────┐");
                Console.WriteLine("│ 1. Add New Note                    │");
                Console.WriteLine("│ 2. View All Notes                  │");
                Console.WriteLine("│ 3. Edit Note                       │");
                Console.WriteLine("│ 4. Delete Note                     │");
                Console.WriteLine("│ 5. Exit                            │");
                Console.WriteLine("└────────────────────────────────────┘");
                Console.Write("\nYour choice: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddNoteFlow(viewModel);
                        break;

                    case "2":
                        ViewNotesFlow(viewModel);
                        break;

                    case "3":
                        EditNoteFlow(viewModel);
                        break;

                    case "4":
                        DeleteNoteFlow(viewModel);
                        break;

                    case "5":
                        Console.WriteLine("Thanks for using Notes App! ");
                        return;

                    default:
                        Console.WriteLine("❌ Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddNoteFlow(NotesViewModel vm)
        {
            vm.AddNoteCommand.Execute(null);

            Console.Write("📝 Title: ");
            vm.NoteTitle = Console.ReadLine();

            Console.Write("📄 Content: ");
            vm.NoteContent = Console.ReadLine();

            if (vm.SaveNoteCommand.CanExecute(null))
            {
                vm.SaveNoteCommand.Execute(null);
                Console.WriteLine("\n✅ Note saved successfully!");
            }
            else
            {
                Console.WriteLine("\n❌ Title cannot be empty!");
            }
        }

        static void ViewNotesFlow(NotesViewModel vm)
        {
            if (vm.Notes.Count == 0)
            {
                Console.WriteLine("📭 No notes yet. Create your first note!");
                return;
            }

            Console.WriteLine("📚 YOUR NOTES:");
            Console.WriteLine("─────────────────────────────────────");

            for (int i = 0; i < vm.Notes.Count; i++)
            {
                var note = vm.Notes[i];
                Console.WriteLine($"\n[{i}] 📌 {note.Title}");
                Console.WriteLine($"    {note.Content}");
                Console.WriteLine($"    🕒 {note.Timestamp:g}");
            }
        }

        static void EditNoteFlow(NotesViewModel vm)
        {
            if (vm.Notes.Count == 0)
            {
                Console.WriteLine(" No notes to edit.");
                return;
            }

            ViewNotesFlow(vm);
            Console.Write("\n✏️  Enter note number to edit: ");

            if (int.TryParse(Console.ReadLine(), out int index) &&
                index >= 0 && index < vm.Notes.Count)
            {
                vm.SelectedNote = vm.Notes[index];

                Console.Write($"New Title (current: {vm.NoteTitle}): ");
                var newTitle = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newTitle))
                    vm.NoteTitle = newTitle;

                Console.Write($"New Content (current: {vm.NoteContent}): ");
                var newContent = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newContent))
                    vm.NoteContent = newContent;

                vm.SaveNoteCommand.Execute(null);
                Console.WriteLine("\n✅ Note updated successfully!");
            }
            else
            {
                Console.WriteLine("❌ Invalid note number.");
            }
        }

        static void DeleteNoteFlow(NotesViewModel vm)
        {
            if (vm.Notes.Count == 0)
            {
                Console.WriteLine("📭 No notes to delete.");
                return;
            }

            ViewNotesFlow(vm);
            Console.Write("\n🗑️  Enter note number to delete: ");

            if (int.TryParse(Console.ReadLine(), out int index) &&
                index >= 0 && index < vm.Notes.Count)
            {
                vm.SelectedNote = vm.Notes[index];

                Console.Write("Are you sure? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    vm.DeleteNoteCommand.Execute(null);
                    Console.WriteLine("\n✅ Note deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\n❌ Deletion cancelled.");
                    vm.CancelEditCommand.Execute(null);
                }
            }
            else
            {
                Console.WriteLine("❌ Invalid note number.");
            }
        }
    }
}