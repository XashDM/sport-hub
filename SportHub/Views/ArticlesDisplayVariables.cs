using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Views
{
    public class ArticlesDisplayVariables
    {
        public int startPosition { get; set; }
        public int amountArticles { get; set; }
        public string? publishValue { get; set; }
        public string? category { get; set; }
        public string? subcategory { get; set; }
        public string? team { get; set; }
        public string? search { get;set; }
    }
}

