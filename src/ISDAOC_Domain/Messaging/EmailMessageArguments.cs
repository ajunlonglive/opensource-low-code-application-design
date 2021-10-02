using System;
using System.Collections.Generic;

namespace Domain.Messaging
{
    public class EmailMessageArguments
    {        
        private readonly Dictionary<ContentTypes, string> _content;

        public EmailMessageArguments()
        {
            _content = new Dictionary<ContentTypes, string>();
        }

        public string this[ContentTypes index]
        {
            get => _content[index];
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (_content.ContainsKey(index))
                {
                    _content[index] = value;
                }
                else
                {
                    _content.Add(index, value);
                }
            }
        }

        public bool HasPlainText => _content.ContainsKey(ContentTypes.Plain) && !string.IsNullOrWhiteSpace(_content[ContentTypes.Plain]);       
        public bool HasHtml => _content.ContainsKey(ContentTypes.Html) && !string.IsNullOrWhiteSpace(_content[ContentTypes.Html]);

        public string RecipientName { get; set; }
        public string RecipientEmailAddress { get; set; }

        public string FromName { get; set; }
        public string FromEmailAddress { get; set; }

        public bool AddReplier { get; set; } = false;

        public string ReplyToName { get; set; } = "BLKCHN Records";
        public string ReplyToEmailAddress { get; set; } = "support@blkchnrecords.com";

        public string Subject { get; set; }
    }
}