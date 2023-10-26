using System.Collections.Generic;
using System.Linq;

namespace TranspTask
{
    class Contour
    {
        private Matrix _transport_tables;
        List<OptimalPoint> _points = new List<OptimalPoint>();
        List<List<OptimalPoint>> _options = new List<List<OptimalPoint>>();

        public Contour(OptimalPoint non_optimal_cell, Matrix transport_tables)
        {
            _transport_tables = transport_tables;
            _points.Add(non_optimal_cell);
        }

        public List<OptimalPoint> DrawContour()
        {
            List<OptimalPoint> potentional_point = new List<OptimalPoint>();

            for (int i = 0; i < _transport_tables.Table.Count; i++)
            {
                if (i != _points[0].Column && _transport_tables.Table[i][_points[0].Row].Occupation)
                {
                    potentional_point.Add(new OptimalPoint(_points[0].Row, i));
                }
            }
            for (int j = 0; j < _transport_tables.Table[0].Count; j++)
            {
                if (j != _points[0].Row && _transport_tables.Table[_points[0].Column][j].Occupation)
                {
                    potentional_point.Add(new OptimalPoint(j, _points[0].Column));
                }
            }

            for (int i = 0; i < potentional_point.Count; i++)
            {
                _points.Add(potentional_point[i]);
                LookContour(_points);
                _points.RemoveAt(_points.Count - 1);
            }

            Remove_IncorrectContours(_options);
            return ReturnShortest(_options);
        }

        private void LookContour(List<OptimalPoint> points)
        {
            if (points.Count >= 4 && (points[0].Column == points[points.Count - 1].Column || points[0].Row == points[points.Count - 1].Row))
            {
                _options.Add(points.GetRange(0, points.Count));
            }
            else
            {
                List<OptimalPoint> potentional_point = new List<OptimalPoint>();

                for (int i = 0; i < _transport_tables.Table.Count; i++)
                {
                    if (_transport_tables.Table[i][_points[_points.Count - 1].Row].Occupation && CheckDuplicates(new OptimalPoint(_points[_points.Count - 1].Row, i), points))
                    {
                        potentional_point.Add(new OptimalPoint(_points[_points.Count - 1].Row, i));
                    }
                }
                for (int j = 0; j < _transport_tables.Table[0].Count; j++)
                {
                    if (_transport_tables.Table[_points[_points.Count - 1].Column][j].Occupation && CheckDuplicates(new OptimalPoint(j, _points[_points.Count - 1].Column), points))
                    {
                        potentional_point.Add(new OptimalPoint(j, _points[_points.Count - 1].Column));
                    }
                }

                for (int i = 0; i < potentional_point.Count; i++)
                {
                    _points.Add(potentional_point[i]);
                    LookContour(_points);
                    _points.RemoveAt(_points.Count - 1);
                }
            }
        }

        private bool CheckDuplicates(OptimalPoint point, List<OptimalPoint> points)
        {
            foreach (OptimalPoint p in points)
            {
                if (p.Row == point.Row && p.Column == point.Column)
                {
                    return false;
                }
            }
            return true;
        }

        private void Remove_IncorrectContours(List<List<OptimalPoint>> contours)
        {
            List<List<OptimalPoint>> correct_contours = new List<List<OptimalPoint>>();

            foreach (List<OptimalPoint> contour in contours)
            {
                int duplicate_row = 1, duplicate_column = 1;
                for (int i = 1; i < contour.Count; i++)
                {
                    if (duplicate_row > 0)
                    {
                        if (contour[i - 1].Row == contour[i].Row)
                        {
                            duplicate_row++;
                            duplicate_column = 1;
                            if (duplicate_row == 3) break;
                        }
                        else
                        {
                            duplicate_row = 1;
                            duplicate_column++;
                            if (duplicate_column == 3) break;
                        }
                    }
                    else if (duplicate_column > 1)
                    {
                        if (contour[i - 1].Column == contour[i].Column)
                        {
                            duplicate_row = 1;
                            duplicate_column++;
                            if (duplicate_column == 3) break;
                        }
                        else
                        {
                            duplicate_row++;
                            duplicate_column = 1;
                            if (duplicate_row == 3) break;
                        }
                    }
                }

                if (duplicate_row < 3 && duplicate_column < 3)
                {
                    correct_contours.Add(contour);
                }
            }

            _options = correct_contours;
        }


        private List<OptimalPoint> ReturnShortest(List<List<OptimalPoint>> contours)
        {
            List<int> lenght_contours = new List<int>();
            foreach (List<OptimalPoint> contour in contours)
            {
                lenght_contours.Add(contour.Count);
            }

            return contours[lenght_contours.IndexOf(lenght_contours.Min())];
        }
    }
}
