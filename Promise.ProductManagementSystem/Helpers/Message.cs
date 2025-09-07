using MimeKit;

namespace Promise.ProductManagementSystem.Helpers
{
   
        public class Message
        {
            public List<MailboxAddress> To { get; set; }
            public string Subject { get; set; }
            public string Content { get; set; }
            public string From { get; set; }
            public Message(IEnumerable<string> to, string subject, string content)
            {
                To = new List<MailboxAddress>();
                To.AddRange(to.Select(x => new MailboxAddress("Coselat", x))); // Fixed: swapped parameters
                Subject = subject;
                Content = content;
            }
        }
    
}
