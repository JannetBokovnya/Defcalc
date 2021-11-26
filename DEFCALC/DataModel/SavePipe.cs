using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DEFCALC.DataModel
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]

    public class SavePipe
    {

        public SavePipe()
        {
            KeysPipes = new List<SavePipeMultiPipes>();
        }


        private string iKeyMg;
        private string inameMg;
        private string ikeyNit;
        private string inameNit;
        private string ikmBegin;
        private string ikmEnd;
        private string ikeyRegion;
        private string inameRegion;
        private string iKeyAkt;
        private string iDataAkt;
        private string iKmAkt;
        private string iNumberPipe;
        private string iNumberAkt;
        private List<SavePipeMultiPipes> pipesField;
        private bool actwihtRegion;
        

        public string KeyMg
        {
            get
            {
                return this.iKeyMg;
            }
            set
            {
                this.iKeyMg = value;
            }
        }

        public string NameMg
        {
            get
            {
                return this.inameMg;
            }
            set
            {
                this.inameMg = value;
            }
        }

        public string KeyNit
        {
            get
            {
                return this.ikeyNit;
            }
            set
            {
                this.ikeyNit = value;
            }
        }


        public string NameNit
        {
            get
            {
                return this.inameNit;
            }
            set
            {
                this.inameNit = value;
            }
        }


        public string KmBegin
        {
            get
            {
                return this.ikmBegin;
            }
            set
            {
                this.ikmBegin = value;
            }
        }

        public string KmEnd
        {
            get
            {
                return this.ikmEnd;
            }
            set
            {
                this.ikmEnd = value;
            }
        }

        public string KeyRegion
        {
            get
            {
                return this.ikeyRegion;
            }
            set
            {
                this.ikeyRegion = value;
            }
        }
        public string NameRegion
        {
            get
            {
                return this.inameRegion;
            }
            set
            {
                this.inameRegion = value;
            }
        }

        public bool ActwihtRegion
        {
            get
            {
                return this.actwihtRegion;
            }
            set
            {
                this.actwihtRegion = value;
            }
        }

        public string KeyAkt
        {
            get
            {
                return this.iKeyAkt;
            }
            set
            {
                this.iKeyAkt = value;
            }
        }

        public string NumberAkt
        {
            get { return this.iNumberAkt; }
            set
            {
                this.iNumberAkt = value;
            }
        }



        public string DataAkt
        {
            get
            {
                return this.iDataAkt;
            }
            set
            {
                this.iDataAkt = value;
            }
        }
        public string KmAkt
        {
            get
            {
                return this.iKmAkt;
            }
            set
            {
                this.iKmAkt = value;
            }
        }
        public string NumberPipe
        {
            get
            {
                return this.iNumberPipe;
            }
            set
            {
                this.iNumberPipe = value;
            }
        }


        [System.Xml.Serialization.XmlElementAttribute("KeysPipes")]
        public List<SavePipeMultiPipes> KeysPipes
        {
            get
            {
                return this.pipesField;
            }
            set
            {
                this.pipesField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SavePipeMultiPipes
    {
        private string _keyPipe;

        public string KeyPipe
        {
            get
            {
                return this._keyPipe;
            }
            set
            {
                this._keyPipe = value;
            }
        }
    }
}
