using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.iOS.Presenters;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Localization;
using UIKit;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Forms.iOS
{
    public abstract class MvxFormsIosSetup : MvxIosSetup
    {
        private List<Assembly> _viewAssemblies;
        private MvxFormsApplication _formsApplication;

        protected MvxFormsIosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected MvxFormsIosSetup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            if (_viewAssemblies == null)
                _viewAssemblies = new List<Assembly>(base.GetViewAssemblies());
            
            return _viewAssemblies;
        }

        protected override void InitializeApp(IMvxPluginManager pluginManager, IMvxApplication app)
        {
            base.InitializeApp(pluginManager, app);
            _viewAssemblies.AddRange(GetViewModelAssemblies());
        }

        public MvxFormsApplication FormsApplication {
            get
            {
                if(_formsApplication == null)
                    _formsApplication = CreateFormsApplication();
                return _formsApplication;
            }
        }

        protected virtual MvxFormsApplication CreateFormsApplication() => new MvxFormsApplication();

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            Xamarin.Forms.Forms.Init();
            return new MvxFormsIosPagePresenter(Window, FormsApplication);
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>(base.ValueConverterAssemblies)
                {
                    typeof(MvxLanguageConverter).Assembly
                };
                return toReturn;
            }
        }

        protected override MvxBindingBuilder CreateBindingBuilder() => new MvxFormsBindingBuilder();
    }
}
