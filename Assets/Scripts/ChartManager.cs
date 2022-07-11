using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class XYPoint
{
    public float X;
    public float Y;
}

public class ChartManager : MonoBehaviour
{
    public ScatterChart chart;
    private List<XYPoint> points;

    private bool doDrawBestFitLine;

    void Start()
    {
        doDrawBestFitLine = false;

        chart = gameObject.GetComponent<ScatterChart>();
        chart.SetSize(750, 450);
        chart.GetOrAddChartComponent<Title>().text = "Starting Height vs Bounce Height";

        chart.RemoveData();
        chart.AddSerie<Scatter>("scatter").symbol.type = SymbolType.Rect;
        
        points = new List<XYPoint>();
    }

    public ScatterChart AddData(float startingHeight, float bounceHeight)
    {
        chart.AddData("scatter", bounceHeight,startingHeight);
        points.Add(new XYPoint() {X = bounceHeight, Y = startingHeight});

        if (doDrawBestFitLine)
            DrawBestFitLine();

        return chart;
    }
    
    public void DrawBestFitLine()
    {
        chart.RemoveSerie<Line>();
        chart.AddSerie<Line>("bestFitLine").symbol.show = false;
        chart.GetSerie("bestFitLine").lineType = LineType.Smooth;

        float k, b;
        double rSquare;

        GenerateLinearBestFit(points, out k, out b, out rSquare);

        float startingX = 0.1f;
        float startingY = k * startingX + (-b);
        float endingX = points.Max(point => point.X);
        float endingY = k * endingX + (-b);

        chart.AddData("bestFitLine", startingX, startingY);
        chart.AddData("bestFitLine", endingX, endingY);

        GameObject.Find("SlopeText").GetComponent<Text>().text = string.Format("Best fit line slope: {0:N3}.  R^2 = {1:N3}", k, rSquare);
        
        doDrawBestFitLine = true;
    }
    
    public static void GenerateLinearBestFit(List<XYPoint> points, out float a, out float b, out double rSquare)
    {
        int numPoints = points.Count;
        float meanX = points.Average(point => point.X);
        float meanY = points.Average(point => point.Y);

        float sumXSquared = points.Sum(point => point.X * point.X);
        float sumYSquared = points.Sum(point => point.Y * point.Y);
        float sumXY = points.Sum(point => point.X * point.Y);
        float sumX = points.Sum(point => point.X);
        float sumY = points.Sum(point => point.Y);

        a = (sumXY / numPoints - meanX * meanY) / (sumXSquared / numPoints - meanX * meanX);
        b = (a * meanX - meanY);
        
        float rNumerator = (numPoints * sumXY) - (sumX * sumY);
        float rDenom = (numPoints * sumXSquared - (sumX * sumX)) * (numPoints * sumYSquared - (sumY * sumY));
        double dblR = rNumerator / Math.Sqrt(rDenom);

        rSquare = dblR * dblR;
    }
}
