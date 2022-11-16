//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();

        public Product FinalProduct { get; set; }
        public bool Cooked { get; private set; } = false;
        private CountdownTimer timer = new CountdownTimer();
        private TimerAdapter timerClient; 

        private class TimerAdapter : TimerClient
        {
            private Recipe recipe;
            public TimerAdapter(Recipe recipe)
            {
               this.recipe = recipe;
            }
            public object TimeOutId { get; }        
            public void TimeOut()
            {
                this.recipe.Cook();
            }
        }

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }

        public int GetCookTime() 
        {
            int result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.Time;
            }
            
            return result;
        }

        // Agregado por ISP

        public void Cook()
        {
            if (this.Cooked) throw new InvalidOperationException("Ya está cocido");
            
            else this.timer.Register(this.GetCookTime(), this.timerClient);
            
        }

    }
}