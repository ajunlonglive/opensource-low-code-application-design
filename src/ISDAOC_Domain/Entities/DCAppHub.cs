//using Domain.Abstractions;
//using System;
//using System.Collections.Generic;

//namespace Domain.Entities
//{
//    public class DCAppHub : Entity
//    {
//        public DCAppHub(Guid id) : base(id)
//        {
           
//        }

//        public Guid DCAppStructureId { get; set; }

//        public DCAppStructure DCAppStructure { get; set; }

        

//        public bool HasChildGroups
//        {
//            get
//            {
//                if (Groups != null && Groups.Count > 0)
//                    return true;
//                else
//                    return false;
//            }
//        }
//    }
//}