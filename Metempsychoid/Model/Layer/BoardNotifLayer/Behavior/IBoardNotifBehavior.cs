using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public interface IBoardNotifBehavior
    {
        void StartNotif(World world);

        bool UpdateNotif(World world);

        void EndNotif(World world);
    }
}
