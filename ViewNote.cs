using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace NotesApp
{
    // VIEWMODEL - Manages state and business logic
    public class NotesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Note> _notes;
        private Note _selectedNote;
        private string _noteTitle;
        private string _noteContent;
        private bool _isEditing;
        private int _nextId = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        // Properties - these notify the UI when they change
        public ObservableCollection<Note> Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }

        public Note SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                if (value != null)
                {
                    NoteTitle = value.Title;
                    NoteContent = value.Content;
                    IsEditing = true;
                }
            }
        }

        public string NoteTitle
        {
            get => _noteTitle;
            set
            {
                _noteTitle = value;
                OnPropertyChanged(nameof(NoteTitle));
            }
        }

        public string NoteContent
        {
            get => _noteContent;
            set
            {
                _noteContent = value;
                OnPropertyChanged(nameof(NoteContent));
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        // Commands - triggered by user actions
        public ICommand AddNoteCommand { get; }
        public ICommand SaveNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand CancelEditCommand { get; }

        // Constructor - initializes everything
        public NotesViewModel()
        {
            Notes = new ObservableCollection<Note>();

            AddNoteCommand = new RelayCommand(AddNote);
            SaveNoteCommand = new RelayCommand(SaveNote, CanSaveNote);
            DeleteNoteCommand = new RelayCommand(DeleteNote, CanDeleteNote);
            CancelEditCommand = new RelayCommand(CancelEdit);
        }

        // Command Methods
        private void AddNote()
        {
            NoteTitle = string.Empty;
            NoteContent = string.Empty;
            IsEditing = true;
            SelectedNote = null;
        }

        private bool CanSaveNote()
        {
            return !string.IsNullOrWhiteSpace(NoteTitle);
        }

        private void SaveNote()
        {
            if (SelectedNote != null)
            {
                // Update existing note
                SelectedNote.Title = NoteTitle;
                SelectedNote.Content = NoteContent;
                SelectedNote.Timestamp = DateTime.Now;

                // Refresh the collection
                var temp = Notes.ToList();
                Notes.Clear();
                foreach (var note in temp)
                    Notes.Add(note);
            }
            else
            {
                // Create new note
                var newNote = new Note(_nextId++, NoteTitle, NoteContent);
                Notes.Insert(0, newNote);
            }

            CancelEdit();
        }

        private bool CanDeleteNote()
        {
            return SelectedNote != null;
        }

        private void DeleteNote()
        {
            if (SelectedNote != null)
            {
                Notes.Remove(SelectedNote);
                CancelEdit();
            }
        }

        private void CancelEdit()
        {
            NoteTitle = string.Empty;
            NoteContent = string.Empty;
            IsEditing = false;
            SelectedNote = null;
        }

        // Notify UI of property changes
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}