/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

// BabBot namespaces
using BabBot.Wow;
using BabBot.Data;
using BabBot.Manager;
using BabBot.Common;


namespace BabBot.Forms.Shared
{
    public partial class RouteDetails : UserControl
    {
        public ComboBox[] CbTypes;
        public TextBox[] ZoneText;

        public event EventHandler RegisterChanges;

        public Dictionary<EndpointTypes, AbstractListEndpoint[]> EndpointList
            = new Dictionary<EndpointTypes, AbstractListEndpoint[]>();

        public RouteDetails()
        {
            InitializeComponent();

            pOptA.Tag = pObjA;
            pObjA.Tag = pSubObjA;
            cbObjA0.Tag = lblObjA0;
            cbObjA1.Tag = lblObjA1;

            pOptB.Tag = pObjB;
            pObjB.Tag = pSubObjB;
            cbObjB0.Tag = lblObjB0;
            cbObjB1.Tag = lblObjB1;

            CbTypes = new ComboBox[] { cbTypeA, cbTypeB };
            ZoneText = new TextBox[] { tbZoneA, tbZoneB };
            Panel[] obj_p = new Panel[] { pOptA, pOptB };

            List<AbstractListEndpoint>[] obj_list = new List<AbstractListEndpoint>[] {
                new List<AbstractListEndpoint>(), new List<AbstractListEndpoint>() };

            foreach (EndpointTypes ept in Enum.GetValues(typeof(EndpointTypes)))
            {
                string class_name = Output.GetLogNameByLfs(
                    Enum.GetName(typeof(EndpointTypes), ept).ToLower(), "") + "ListEndpoint";

                Type reflect_class = Type.GetType("BabBot.Forms.Shared." + class_name);

                AbstractListEndpoint[] ale = new AbstractListEndpoint[2];
                for (int i = 0; i < 2; i++)
                {
                    AbstractListEndpoint epl = Activator.CreateInstance(
                        reflect_class, new object[] { this, 
                            obj_p[i], (char) (65 + i), ept }) as AbstractListEndpoint;
                    obj_list[i].Add(epl);
                    ale[i] = epl;
                }

                EndpointList.Add(ept, ale);
            }

            for (int i = 0; i < 2; i++)
            {
                CbTypes[i].Tag = i;
                CbTypes[i].DataSource = obj_list[i];
                CbTypes[i].DisplayMember = "Name";
                CbTypes[i].SelectedIndex = (int)EndpointTypes.UNDEF;
            }
        }

        private void bsGameObjectsA_CurrentChanged(object sender, EventArgs e)
        {
            if (bsGameObjectsA.Current == null)
                return;

            if (cbObjA0.DataSource == bsGameObjectsA)
                bsGameObjectsB.Filter = "ID <> " +
                    ((DataRowView)bsGameObjectsA.Current).Row["ID"].ToString();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbs = (ComboBox)sender;
            if (cbs.SelectedItem == null)
                return;

            ((AbstractListEndpoint)cbs.SelectedItem).OnSelection();

            OnRegisterChanges(sender, e);
        }

        private void OnRegisterChanges(object sender, EventArgs e)
        {
            if (RegisterChanges != null)
                RegisterChanges(sender, e);
        }

        private void cbObjA0_DataSourceChanged(object sender, EventArgs e)
        {
            if (cbObjA0.DataSource != bsGameObjectsA)
                bsGameObjectsB.Filter = null;
            else
                bsGameObjectsA_CurrentChanged(sender, e);
        }

        public void SetDataSource(BotDataSet source)
        {
            bsGameObjectsA.DataSource = source;
            bsGameObjectsB.DataSource = source;

            bsQuestListA.DataSource = source;
            bsQuestListB.DataSource = source;
        }

        public void PopulateControls(Route r)
        {
            for (int i = 0; i < 2; i++)
            {
                Endpoint ep = r[i];
                AbstractListEndpoint ab = EndpointList[ep.PType][i];

                ZoneText[i].Text = ep.ZoneText;
                CbTypes[i].Text = ab.Name;

                ab.OnSelection(r[i]);
            }

            tbDescr.Text = r.Description;
            cbReversible.Checked = r.Reversible;
            lblWaypointFile.Text = "Waypoint : " + 
                                r.WaypointFileName + ".wp";
        }

        public bool CheckEndpoints()
        {
            for (int i = 0; i < 2; i++)
            {
                ComboBox cb = CbTypes[i];
                if (cb.SelectedItem == null)
                {
                    GenericDialog.ShowErrorMessage(this,
                        "Type for Endpoint " + (char)(65 + i) + " not selected");
                    return false;
                }
            }

            return true;
        }

        public Route GetRoute(Vector3D a, Vector3D b)
        {
            // Check if B is set
            if (!(CheckEndpoints() &&
                ((AbstractListEndpoint)cbTypeA.SelectedItem).Check &&
                ((AbstractListEndpoint)cbTypeB.SelectedItem).Check))
                    return null;

            // Make route
            AbstractListEndpoint point_a = cbTypeA.SelectedItem as AbstractListEndpoint;
            AbstractListEndpoint point_b = cbTypeB.SelectedItem as AbstractListEndpoint;

            // Make new route
            Route route = new Route(
                    point_a.GetEndpoint(tbZoneA.Text, a),
                    point_b.GetEndpoint(tbZoneB.Text, b),
                        tbDescr.Text, cbReversible.Checked);

            return route;
        }
    }

    #region Classes for Route Endpoints

    public abstract class AbstractListEndpoint
    {
        public virtual bool Check
        {
            get { return true; }
        }

        public abstract EndpointTypes EType { get; }

        public abstract string Name { get; }

        public abstract void OnSelection();
        public abstract void OnSelection(Endpoint ep);

        public abstract Endpoint GetEndpoint(string ZoneText, Vector3D waypoint);
    }

    public class UndefListEndpoint : AbstractListEndpoint
    {
        protected char C;
        protected Panel[] PTargets = new Panel[3];

        protected RouteDetails Owner;
        protected ComboBox[] cb_list = new ComboBox[2];

        private EndpointTypes _etype;

        public override string Name
        {
            get { return "undef"; }
        }

        public override EndpointTypes EType
        {
            get { return _etype; }
        }

        public UndefListEndpoint() { }

        public UndefListEndpoint(RouteDetails owner, Panel target, char a_b, EndpointTypes etype)
        {
            C = a_b;
            Owner = owner;
            _etype = etype;
            PTargets[0] = target;

            for (int i = 0; i < 2; i++)
            {
                PTargets[i + 1] = (Panel)PTargets[i].Tag;
                cb_list[i] = (ComboBox)PTargets[i + 1].Controls["cbObj" + C + i];
            }
        }

        public override bool Check
        {
            get
            {
                return GenericDialog.GetConfirmation(Owner,
                  "Continue with undefined Endpoint " + C + " ?");
            }
        }

        public override void OnSelection()
        {
            OnSelection(null);
        }

        public override void OnSelection(Endpoint ep)
        {
            SetControls(false, 2);
            foreach (ComboBox cb in cb_list)
                cb.DataSource = null;
        }

        protected void SetControls(bool enabled, int cnt)
        {
            for (int i = 0; i < cnt; i++)
                PTargets[i + 1].Visible = enabled;
        }

        public override Endpoint GetEndpoint(string ZoneText, Vector3D waypoint)
        {
            return new Endpoint(EType, ZoneText, waypoint);
        }
    }

    public class DefListEndpoint : UndefListEndpoint
    {
        public DefListEndpoint(RouteDetails owner, 
                        Panel target, char a_b, EndpointTypes etype)
            : base(owner, target, a_b, etype) { }

        public override bool Check
        {
            get { return true; }
        }
    }

    public abstract class AbstractDefListEndpoint : UndefListEndpoint
    {
        protected readonly BindingSource[] bs;
        private string[] DisplayMembers;
        private int _bs_cnt;
        protected abstract string TypeStr { get; }

        public AbstractDefListEndpoint(string bs_name) : base() { }

        public AbstractDefListEndpoint(RouteDetails owner, Panel target, 
                char a_b, EndpointTypes etype, string[] bs_list, string[] members)
            : base(owner, target, a_b, etype)
        {
            _bs_cnt = bs_list.Length;
            DisplayMembers = members;
            bs = new BindingSource[_bs_cnt];

            for (int i = 0; i < bs_list.Length; i++)
                bs[i] = FindBindingSource(bs_list[i]);
        }

        private BindingSource FindBindingSource(string name)
        {
            // Check for binding source
            FieldInfo[] fi = Owner.GetType().GetFields();

            foreach (FieldInfo f in fi)
                if (f.Name.Equals(name + C))
                    return (BindingSource)f.GetValue(Owner);

            return null;
        }

        public override bool Check
        {
            get
            {
                if (cb_list[0].SelectedItem == null)
                {
                    GenericDialog.ShowErrorMessage(Owner,
                        "Select " + TypeStr + " for Point " + C);
                    return false;
                }

                return true;
            }
        }

        public override void OnSelection(Endpoint ep)
        {
            base.OnSelection(ep);
            SetControls(true, _bs_cnt);

            for (int i = 0; i < _bs_cnt; i++)
                SetBindingSource(cb_list[i], i, DisplayMembers[i]);

        }

        protected void SetBindingSource(ComboBox cb, int idx, string member)
        {
            cb.DataSource = bs[idx];
            cb.DisplayMember = member;
            // For now it's static
            ((Label)cb.Tag).Text = DisplayMembers[idx][0] +
                    DisplayMembers[idx].Substring(1).ToLower();
        }
    }

    public class GameObjListEndpoint : AbstractDefListEndpoint
    {
        public GameObjListEndpoint(RouteDetails owner, 
                                Panel target, char a_b, EndpointTypes etype)
            : base(owner, target, a_b, etype, new string[] { "bsGameObjects" },
                                                new string[] { "NAME" }) { }

        public override string Name
        {
            get { return "game_object"; }
        }

        public override Endpoint GetEndpoint(string ZoneText, Vector3D waypoint)
        {
            return new GameObjEndpoint(EType, cb_list[0].Text, ZoneText, waypoint);
        }

        protected override string TypeStr
        {
            get { return "Game Object"; }
        }

        public override void OnSelection(Endpoint ep)
        {
            base.OnSelection(ep);
            if (ep != null)
                cb_list[0].Text = ((GameObjEndpoint)ep).GameObjName;
        }
    }

    public class QuestObjListEndpoint : AbstractDefListEndpoint
    {
        public QuestObjListEndpoint(RouteDetails owner, 
                                    Panel target, char a_b, EndpointTypes etype)
            : base(owner, target, a_b, etype,
                new string[] { "bsQuestList", "fkQuestItems" },
                            new string[] { "TITLE", "NAME" }) { }

        public override string Name
        {
            get { return "quest_objective"; }
        }

        public override bool Check
        {
            get
            {
                if (!base.Check || (cb_list[1].SelectedItem == null))
                {
                    GenericDialog.ShowErrorMessage(Owner,
                        "Select Quest Objective for Point " + C);
                    return false;
                }

                return true;
            }
        }

        protected override string TypeStr
        {
            get { return "Quest"; }
        }

        public override Endpoint GetEndpoint(string ZoneText, Vector3D waypoint)
        {
            BotDataSet.QuestListRow q_row = (BotDataSet.QuestListRow)((DataRowView)
                ((BindingSource)cb_list[0].DataSource).Current).Row;
            BotDataSet.QuestItemsRow obj_row = (BotDataSet.QuestItemsRow)((DataRowView)
                ((BindingSource)cb_list[1].DataSource).Current).Row;
            return new QuestObjEndpoint(EType, q_row.ID, obj_row.IDX, ZoneText, waypoint);
        }

        public override void OnSelection(Endpoint ep)
        {
            base.OnSelection(ep);
            if (ep != null)
            {
                QuestObjEndpoint qi = (QuestObjEndpoint)ep;
                Quest q = DataManager.FindQuestById(qi.QuestId);
                
                if (q != null)
                {
                    cb_list[0].Text = q.Title;
                    cb_list[1].Text = q.Objectives.ObjList[qi.ObjId].Name;
                }
            }
        }
    }

    public class HotSpotListEndpoint : DefListEndpoint
    {
        public HotSpotListEndpoint(RouteDetails owner, 
                        Panel target, char a_b, EndpointTypes etype)
            : base(owner, target, a_b, etype) { }

        public override string Name
        {
            get { return "hot_spot"; }
        }
    }

    public class GraveyardListEndpoint : DefListEndpoint
    {
        public GraveyardListEndpoint(RouteDetails owner, 
                            Panel target, char a_b, EndpointTypes etype)
            : base(owner, target, a_b, etype) { }

        public override string Name
        {
            get { return "graveyard"; }
        }
    }

    #endregion

}
