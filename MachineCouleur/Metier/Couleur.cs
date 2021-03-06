﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MachineCouleur.Metier
{
    public class Couleur : INotifyPropertyChanged
    {
        #region Constantes

        public const int ROUGE = 0;
        public const int VERT = 1;
        public const int BLEU = 2;

        private const byte VALEUR_MAX = 255;

        #endregion

        #region Variables

        private Random _Rand = new Random(DateTime.Now.Millisecond);


        #endregion

        #region Accesseurs

        private string _Nom;
        public string Nom
        {
            get
            {
                return _Nom;
            }
        }

        private byte _Rouge;
        public byte Rouge {
            get { return _Rouge; }
            set { _Rouge = LimiterValeur(value); }
        }

        private byte _Vert;
        public byte Vert {
            get { return _Vert; }
            set { _Vert = LimiterValeur(value); }
        }

        private byte _Bleu;
        public byte Bleu {
            get { return _Bleu; }
            set { _Bleu = LimiterValeur(value); }
        }

        private Color _RVB;
        public Color RVB
        {
            get
            {
                return _RVB;
            }
        }

        SolidColorBrush _BrushCouleur;
        public SolidColorBrush BrushCouleur
        {
            get
            {
                return _BrushCouleur;
            }
        }

        #endregion

        #region Constructeur

        public Couleur()
        {
            SetProprietes(0, 0, 0);
        }

        #endregion

        #region Interface

        public void Aleatoire()
        {
            SetProprietes(NumberRand(), NumberRand(), NumberRand());
        }

        #endregion

        #region Implémentation

        private byte LimiterValeur(byte pValue)
        {
            if (pValue < 0) return 0;
            if (pValue > VALEUR_MAX) return VALEUR_MAX;

            return pValue;
        }

        private byte NumberRand()
        {
            return (byte)_Rand.Next(VALEUR_MAX + 1) ;
        }

        private void SetProprietes(byte pRouge, byte pVert, byte pBleu)
        {
            Rouge = pRouge;
            Vert = pVert;
            Bleu = pBleu;

            _BrushCouleur = new SolidColorBrush(Color.FromArgb(255, Rouge, Vert, Bleu));
            OnPropertyChanged(new PropertyChangedEventArgs("BrushCouleur"));

            _RVB = _BrushCouleur.Color; 
            OnPropertyChanged(new PropertyChangedEventArgs("RVB"));

            _Nom = _RVB.ToString();
            OnPropertyChanged(new PropertyChangedEventArgs("Nom"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion
    }
}
