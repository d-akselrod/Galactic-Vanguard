using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galactic_Vanguard
{
    public class EnvironmentListener
    {
        private Space space;

        public EnvironmentListener(Space space)
        {
            this.space = space;
        }

        public void AddBullet(Bullet bullet)
        {
             space.AddEntity(bullet);
        }
    }
}