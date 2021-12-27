using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiremelonEditor2
{
    public class UtilityFactory : IUtilityFactory
    {
        public IBitmapUtility NewBitmapUtility()
        {
            return new BitmapUtility();
        }

        public IDrawingUtility NewDrawingUtility()
        {
            return new DrawingUtility();
        }

        public ILinearAlgebraUtility NewLinearAlgebraUtility()
        {
            return new LinearAlgebraUtility();
        }

        public INameGenerator NewNameGenerator()
        {
            return new NameGenerator();
        }

        public INameUtility NewNameUtility()
        {
            return new NameUtility();
        }

        public INameValidator NewNameValidator(INameGenerator nameGenerator)
        {
            return new NameValidator(nameGenerator);
        }

        public IProjectUtility NewProjectUtility()
        {
            return new ProjectUtility();
        }
        
        public IUriUtility NewUriUtility()
        {
            return new UriUtility();
        }
    }

    public interface IUtilityFactory
    {
        IBitmapUtility          NewBitmapUtility();

        IDrawingUtility         NewDrawingUtility();

        ILinearAlgebraUtility   NewLinearAlgebraUtility();

        INameGenerator          NewNameGenerator();

        INameUtility            NewNameUtility();

        INameValidator          NewNameValidator(INameGenerator nameGenerator);

        IProjectUtility         NewProjectUtility();

        IUriUtility             NewUriUtility();
    }
}
