using System.Collections.Generic;

namespace TranspTask
{
    class Transport
    {
        private InformationHandler _information_handler;

        private int _N;
        private bool _optimal;

        public Transport(InformationHandler information_handler)
        {
            _information_handler = information_handler;
        }

        public InformationHandler StartAlgorithm()
        {
            FirstBoot firstBoot = new FirstBoot(_information_handler.Matrix, _information_handler.Stocks, _information_handler.Demands);
            firstBoot.FillMatrix();

            _N = FirstBoot.N;
            FindSolution();

            return _information_handler;
        }

        private void FindSolution()
        {
            Optimality optimality = new Optimality(_information_handler.Matrix);
            OptimalPoint point = optimality.Select_WorstOption();
            _optimal = optimality.Optimal;

            if (_optimal == false)
            {
                Contour contour = new Contour(point, _information_handler.Matrix);
                List<OptimalPoint> contour_points = contour.DrawContour();

                UpdatingTable updatingTable = new UpdatingTable(contour_points, _information_handler.Matrix, _N);
                updatingTable.UpdateTable();

                FindSolution();
            }
        }
    }
}
