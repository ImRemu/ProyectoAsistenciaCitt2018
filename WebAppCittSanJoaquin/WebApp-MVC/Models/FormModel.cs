using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Recursos;
namespace WebApp_MVC.Models
{
    public class FormModel
    {
        #warning Aqui se usa un archivo de recursos!!
        /** 
        * Asignamos el label por defecto de cada propiedad.
        * ademas, tenemos que dejar definido que archivo de
        * recurso usamos. 
        * Los archivos de recursos, en codigo, son clases estaticas,
        * por eso no hay ni una instanciación
        **/
        [Required]
        [Display(Name = "Campo1Label", ResourceType = typeof(Recursos.Resources))]
        public string TextBoxStringData { get; set; }

        [Required]
        [Display(Name = "Campo2Label", ResourceType = typeof(Recursos.Resources))]
        public int TextBoxIntData { get; set; }

        [Required]
        [Display(Name = "Campo3Label", ResourceType = typeof(Recursos.Resources))]
        public bool CheckBoxData { get; set; }
    }
}