using System.Collections.Generic;
using Domain.Messaging;

namespace Infrastructure.Messaging.EmailMessageBuilders
{
    public class TrackForReviewEmail
    {
        private readonly StringInterpolator _interpolator;
        private readonly Dictionary<ContentTypes, string> _templates;

        public TrackForReviewEmail()
        {
            _interpolator = new StringInterpolator();
            _templates = new Dictionary<ContentTypes, string>();
        }

        public EmailMessageArguments Construct(string recipientEmail, string trackTitle, string artistName)
        {
            var args = new EmailMessageArguments
            {
                FromName = "Valstekt",
                FromEmailAddress = "noreply@app.valstekt.com",
                Subject = "A track is ready for review: " + @trackTitle,
                AddReplier = false,
                RecipientName = recipientEmail,
                RecipientEmailAddress = recipientEmail
            };

            _interpolator.Add("@trackTitle", trackTitle);
            _interpolator.Add("@artistName", artistName);

            if (_templates.ContainsKey(ContentTypes.Html))
            {
                var template = _templates[ContentTypes.Html];
                var markdown = _interpolator.Interpolate(template);
                var htmlBuilder = new HtmlContentBuilder().WithMarkdown(markdown);
                args[ContentTypes.Html] = htmlBuilder.Build();
            }

            if (_templates.ContainsKey(ContentTypes.Plain))
            {
                var textTemplate = _interpolator.Interpolate(_templates[ContentTypes.Html]);
                args[ContentTypes.Plain] = textTemplate;
            }

            return args;
        }

        public void Add(ContentTypes contentType, string template)
        {
            _templates.Add(contentType, template);
        }
    }
}