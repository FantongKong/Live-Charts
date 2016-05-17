﻿//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

namespace LiveCharts.SeriesAlgorithms
{
    public class PieAlgorithm : SeriesAlgorithm, IPieSeries
    {
        public PieAlgorithm(ISeriesView view) : base(view)
        {
        }

        public override void Update()
        {
            var fy = CurrentYAxis.GetFormatter();

            var pieChart = (IPieChart) View.Model.Chart.View;

            var minDimension = Chart.DrawMargin.Width < Chart.DrawMargin.Height
                ? Chart.DrawMargin.Width
                : Chart.DrawMargin.Height;
            minDimension -= 10;
            minDimension = minDimension < 10 ? 10 : minDimension;
            
            var inner = pieChart.InnerRadius;

            foreach (var chartPoint in View.Values.Points)
            {
                chartPoint.View = View.GetPointView(chartPoint.View, chartPoint,
                    View.DataLabels ? fy(chartPoint.Y) : null);

                var pieSlice = (IPieSlicePointView) chartPoint.View;

                var space = pieChart.InnerRadius +
                            ((minDimension/2) - pieChart.InnerRadius)*((chartPoint.X + 1)/(View.Values.XLimit.Max + 1));

                chartPoint.SeriesView = View;

                pieSlice.Radius = space;
                pieSlice.InnerRadius = inner;
                pieSlice.Rotation = (chartPoint.StackedParticipation - chartPoint.Participation)*360;
                pieSlice.Wedge = chartPoint.Participation*360 > 0 ? chartPoint.Participation*360 : 0;

                chartPoint.View.DrawOrMove(null, chartPoint, 0, Chart);

                inner += space;
            }
        }
    }
}
