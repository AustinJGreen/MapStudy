using System;
using System.Collections.Generic;
using System.Windows.Forms;

using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace MapStudy
{
    public partial class StatisticsForm : Form
    {
        private PlotView view;
        private PlotModel model;

        public StatisticsForm()
        {
            InitializeComponent();

            view = new PlotView();

            model = new PlotModel();
            model.Title = "Map Learning";

            LinearAxis yAxis = new LinearAxis();
            yAxis.Title = "Percent Learned";
            yAxis.Position = AxisPosition.Left;
            yAxis.Minimum = 0;
            yAxis.Maximum = 100;

            DateTimeAxis xAxis = new DateTimeAxis();
            xAxis.Title = "Time";
            xAxis.Position = AxisPosition.Bottom;

            model.Axes.Add(yAxis);
            model.Axes.Add(xAxis);

            view.Model = model;
            view.Dock = DockStyle.Fill;

            Controls.Add(view);
        }

        public void LoadStats(Dictionary<DateTime, Statistic[]> stats)
        {
            //statsControl1.LoadStats(stats);

            LineSeries locationSeries = new LineSeries();
            locationSeries.Title = "Location";
            locationSeries.LineStyle = LineStyle.DashDashDotDot;
            locationSeries.BrokenLineThickness = 1;

            LineSeries countrySeries = new LineSeries();
            countrySeries.Title = "Country";
            countrySeries.LineStyle = LineStyle.DashDashDotDot;
            countrySeries.BrokenLineThickness = 1;

            LineSeries capitalSeries = new LineSeries();
            capitalSeries.Title = "Capital";
            capitalSeries.LineStyle = LineStyle.DashDashDotDot;
            capitalSeries.BrokenLineThickness = 1;

            foreach (KeyValuePair<DateTime, Statistic[]> stat in stats)
            {
                DateTime time = stat.Key;
                Statistic[] s = stat.Value;      

                double locationLearned = 0;
                double countryLearned = 0;
                double capitalLearned = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    double lLearn = s[i].GetLocationCompletion();
                    locationLearned += lLearn;

                    double coLearn = s[i].GetCountryCompletion();
                    countryLearned += coLearn;

                    double caLearn = s[i].GetCapitalCompletion();
                    capitalLearned += caLearn;
                }

                locationLearned /= s.Length;
                countryLearned /= s.Length;
                capitalLearned /= s.Length;

                locationSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), locationLearned * 100)); // * 100 ???
                countrySeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), countryLearned * 100)); // * 100 ???
                capitalSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), capitalLearned * 100)); // * 100 ???
            }

            model.Series.Add(locationSeries);
            model.Series.Add(countrySeries);
            model.Series.Add(capitalSeries);

            Invalidate();
        }
    }
}
