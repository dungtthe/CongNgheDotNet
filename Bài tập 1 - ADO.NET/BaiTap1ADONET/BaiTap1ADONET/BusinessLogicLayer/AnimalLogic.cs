using BaiTap1ADONET.DataAccessLayer.DataClass;
using BaiTap1ADONET.DataAccessLayer.Object;
using BaiTap1ADONET.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTap1ADONET.BusinessLogicLayer
{
    public class AnimalLogic
    {
        private AnimalDAL _animalDAL = AnimalDAL.GI();

        private static AnimalLogic ins;
        public static AnimalLogic GI()
        {
            if(ins == null)
            {
                ins= new AnimalLogic();
            }
            return ins;
        }

        public List<Animal> GetAllAnimals()
        {
            return _animalDAL.GetAllAnimals();
        }

        public List<DataThongKe> Statistical(int cowAmount, int sheepAmount, int goatAmount)
        {
            List<Animal> animals = _animalDAL.GetAllAnimals();
            List<DataThongKe> data = new List<DataThongKe>();

            foreach (Animal animal in animals)
            {
                DataThongKe dt = new DataThongKe();
                switch (animal.AnimalType.ToLower())
                {
                    case "cow":
                        dt.CurrentAmount = cowAmount;
                        dt.TotalMilk = ProduceMilk(animal, cowAmount);
                        dt.TotalGiveBirth = GiveBirth(animal, cowAmount);
                        break;
                    case "sheep":
                        dt.CurrentAmount = sheepAmount;
                        dt.TotalMilk = ProduceMilk(animal, sheepAmount);
                        dt.TotalGiveBirth = GiveBirth(animal, sheepAmount);
                        break;
                    case "goat":
                        dt.CurrentAmount = goatAmount;
                        dt.TotalMilk = ProduceMilk(animal, goatAmount);
                        dt.TotalGiveBirth = GiveBirth(animal, goatAmount);
                        break;
                }
                dt.AnimalType = animal.AnimalType;
                dt.TotalAmount = dt.CurrentAmount + dt.TotalGiveBirth;

                data.Add(dt);
            }

            return data;
        }

        public int ProduceMilk(Animal animal, int count)
        {
            int totalMilk = 0;
            for (int i = 0; i < count; i++)
            {

                totalMilk += animal.ProduceMilk();
            }
            return totalMilk;
        }

        public int GiveBirth(Animal animal, int count)
        {
            int totalGiveBirth = 0;
            for (int i = 0; i < count; i++)
            {
                totalGiveBirth += animal.GiveBirth();
            }
            return totalGiveBirth;
        }
    }
}
