﻿using System;
using PowerLanguage.Function;

namespace PowerLanguage.Strategy
{
    public class MovAvg2Line_Cross_LE : SignalObject
    {
        private AverageFC m_FastAverageFC;

        private AverageFC m_SlowAverageFC;

        private VariableSeries<Double> m_FastAvg;

        private VariableSeries<Double> m_SlowAvg;

        private IOrderMarket m_MA2CrossLE;

        public MovAvg2Line_Cross_LE(object ctx) :
            base(ctx)
        {
            SlowLength = 18;
            FastLength = 9;
        }

        private ISeries<double> Price { get; set; }

        [Input]
        public int FastLength { get; set; }

        [Input]
        public int SlowLength { get; set; }

        protected override void Create(){
            m_FastAverageFC = new AverageFC(this);
            m_SlowAverageFC = new AverageFC(this);
            m_FastAvg = new VariableSeries<Double>(this);
            m_SlowAvg = new VariableSeries<Double>(this);
            m_MA2CrossLE =
                OrderCreator.MarketNextBar(new SOrderParameters(Contracts.Default, "MA2CrossLE", EOrderAction.Buy));
        }

        protected override void StartCalc(){
            Price = Bars.Close;
            m_FastAverageFC.price = Price;
            m_FastAverageFC.length = FastLength;
            m_SlowAverageFC.price = Price;
            m_SlowAverageFC.length = SlowLength;
         }


        protected override void CalcBar(){
            m_FastAvg.Value = m_FastAverageFC[0];
            m_SlowAvg.Value = m_SlowAverageFC[0];
            if (Bars.CurrentBar > 1 && m_FastAvg.CrossesOver(m_SlowAvg, ExecInfo.MaxBarsBack))
            {
                m_MA2CrossLE.Send();
            }
        }
    }
}