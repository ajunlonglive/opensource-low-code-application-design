using Domain.Abstractions;
using Markdig;

namespace Infrastructure.Messaging
{
    public class HtmlContentBuilder : IBuilder<string>
    {
        private string _content;
        private readonly MarkdownPipeline _pipeline;

        public HtmlContentBuilder(MarkdownPipelineBuilder pipelineBuilder = null)
        {
            _pipeline = pipelineBuilder == null ? new MarkdownPipelineBuilder().UseAdvancedExtensions().Build() : pipelineBuilder.Build();
        }

        public HtmlContentBuilder WithMarkdown(string value)
        {
            _content = value;
            return this;
        }

        public string Build()
        {
            return Markdown.ToHtml(_content, _pipeline);
        }
    }
}