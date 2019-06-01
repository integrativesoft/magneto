/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using Integrative.Lara;
using System.Globalization;
using System.Threading.Tasks;

namespace SampleApp
{
    class SelectSample
    {
        readonly Select _select;
        readonly Button _advance;

        public SelectSample()
        {
            _select = new Select
            {
                Id = "myselect",
                Class = "form-control",
            };
            _advance = new Button
            {
                Class = "btn btn-primary ml-2"
            };
            _advance.AppendChild(new TextNode("Advance"));
            _select.AddOption("0", "Monday");
            _select.AddOption("1", "Tuesday");
            _select.AddOption("2", "Wednesday");
            _select.AddOption("3", "Thursday");
            _select.AddOption("4", "Friday");
            _select.AddOption("5", "Saturday");
            _select.AddOption("6", "Sunday");
            _advance.On("click", app =>
            {
                int.TryParse(_select.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int weekday);
                weekday = (weekday + 1) % 7;
                _select.Value = weekday.ToString(CultureInfo.InvariantCulture);
                return Task.CompletedTask;
            });
        }

        public Element Build()
        {
            var row = Element.Create("div");
            row.Class = "form-row";

            var builder = new LaraBuilder(row);
            builder
            .Push("div", "form-group")
                .Push(_select)
                .Pop()
            .Pop()
            .Push("div", "form-group")
                .Push(_advance)
                .Pop()
            .Pop();

            return row;
        }

    }
}
