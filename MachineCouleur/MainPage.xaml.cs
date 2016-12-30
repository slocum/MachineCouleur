using MachineCouleur.Metier;
using SloIALib.ANNs.PMLs;
using SloIALib.ANNs.PMLs.Donnees;
using SloIALib.ANNs.PMLs.SloReseauNeurones;
using SloIALib.Functions;
using SloOutilsLib.DossiersFichiers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MachineCouleur
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Variables

        private LstEntrees _LstEntrees;
        private LstSorties _LstSorties;
        private Couleur _Couleur;

        private ReseauNeurones _RN;
        private MachineAApprendre _MaA;
        private double _TxApprentissage;

        #endregion

        #region Constructeur

        public MainPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Implémentation

        private void Init()
        {
            CreerListeEntrees();
            CreerListeSorties();

            _Couleur = new Couleur();
            this.DataContext = _Couleur;

            CreerReseauNeurones();
        }

        private void CreerListeEntrees()
        {
            if (_LstEntrees == null)
            {
                _LstEntrees = new LstEntrees();

                _LstEntrees.Ajouter(new Entree("Rouge"));
                _LstEntrees.Ajouter(new Entree("Vert"));
                _LstEntrees.Ajouter(new Entree("Bleu"));

                lstEntrees.ItemsSource = _LstEntrees.Observable;

            }
        }

        private void CreerListeSorties()
        {
            if (_LstSorties == null)
            {
                _LstSorties = new LstSorties();

                _LstSorties.Ajouter(new Sortie("Inconnue", true));
                _LstSorties.Ajouter(new Sortie("Noir"));
                _LstSorties.Ajouter(new Sortie("Blanc"));
                _LstSorties.Ajouter(new Sortie("Gris"));
                _LstSorties.Ajouter(new Sortie("Rouge"));
                _LstSorties.Ajouter(new Sortie("Vert"));

                lstSorties.ItemsSource = _LstSorties.Observable;
            }
        }

        private void CreerReseauNeurones()
        {
            _TxApprentissage = 0.7;

            _RN = new ReseauNeurones(_LstEntrees.Count, new int[] {5, _LstSorties.Count }, new SigmoidFunction());
            _MaA = new MachineAApprendre(_RN, _TxApprentissage);
        }

        private void NouvelleCouleur()
        {
            _Couleur.Aleatoire();

            _LstEntrees.Modifier(Couleur.ROUGE, _Couleur.Rouge);
            _LstEntrees.Modifier(Couleur.VERT, _Couleur.Vert);
            _LstEntrees.Modifier(Couleur.BLEU, _Couleur.Bleu);

        }

        private void NouvelEchantillon()
        {
            NouvelleCouleur();

            _LstSorties.AjouterLstDouble(_RN.CalculerSorties(_LstEntrees.Doubles));
        }
        #endregion

        #region Gestionnaire d'événements

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init();

            base.OnNavigatedTo(e);
        }

        private void Btndemarrer_Click(object sender, RoutedEventArgs e)
        {
            NouvelEchantillon();
        }

        #endregion

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            string nom = radioBtn.Content.ToString();

            double[] sorties = new double[_LstSorties.Count];

            for (int i = 0; i < _LstSorties.Count; i++)
            {
                sorties[i] = 0;
                if (_LstSorties.Observable[i].Nom == nom )
                {
                    sorties[i] = 1;
                }
            }

            Echantillon echantillon = new Echantillon(_LstEntrees.Doubles, sorties);
            _MaA.Apprendre(echantillon);

            NouvelEchantillon();
        }
    }
}
