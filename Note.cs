using System;

namespace NotesApp
{
    // MODEL - Represents a single note
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        public Note(int id, string title, string content)
        {
            Id = id;
            Title = title;
            Content = content;
            Timestamp = DateTime.Now;
        }
    }
}