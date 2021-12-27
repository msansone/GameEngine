using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiremelonEditor2
{
    using ProjectDto = ProjectDto_2_2;

    public partial class ParticleEmittersEditorControl : UserControl, IParticleEmittersEditorControl
    {
        #region Constructors

        public ParticleEmittersEditorControl(IProjectController projectController, INameGenerator nameGenerator, IExceptionHandler exceptionHandler)
        {
            InitializeComponent();

            nameGenerator_ = nameGenerator;

            projectController_ = projectController;

            exceptionHandler_ = exceptionHandler;

            firemelonEditorFactory_ = new FiremelonEditorFactory();

            scriptGeneratorFactory_ = new ScriptGeneratorFactory();

            utilityFactory_ = new UtilityFactory();

            //Root nodes.
            tvParticleEmitters.Nodes.Add("PARTICLEEMITTERROOT", "Partricle Emitters");//.Tag = new AssetMenuDto(cmnuActor, null);

            tvParticleEmitters.Nodes.Add("PARTICLEROOT", "Particles");///.Tag = new AssetMenuDto(cmnuEvent, null);
            
            pythonScriptEditorControl_ = firemelonEditorFactory_.NewPythonScriptEditorControl(projectController);

            ((Control)pythonScriptEditorControl_).Dock = DockStyle.Fill;

            scParticleEmitters.Panel2.Controls.Add((Control)pythonScriptEditorControl_);

            ((Control)pythonScriptEditorControl_).Visible = false;
            
            ProjectDto project = projectController.GetProjectDto();

            // Build the tree.
            foreach (ParticleEmitterDto particleEmitter in project.ParticleEmitters)
            {
                TreeNode particleEmitterNode = addParticleEmitterToTree(particleEmitter);
            }

            foreach (ParticleDto particle in project.Particles)
            {
                TreeNode particleNode = addParticleToTree(particle);                
            }            
        }

        #endregion

        #region Private Variables

        private IExceptionHandler exceptionHandler_;

        private IFiremelonEditorFactory firemelonEditorFactory_;

        private INameGenerator nameGenerator_;

        private IProjectController projectController_;

        private IPythonScriptEditorControl pythonScriptEditorControl_;

        private IScriptGeneratorFactory scriptGeneratorFactory_;

        private Guid selectedEntityId_;
        
        private IUtilityFactory utilityFactory_;

        #endregion

        #region Public Functions

        public void AddNew()
        {
            addNewParticleEmitter();
        }

        public void AddNewParticle()
        {
            addNewParticle();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void DeleteParticle()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Private Functions

        private void addNewParticle()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int actorCount = tvParticleEmitters.Nodes["PARTICLEROOT"].Nodes.Count + 1;

            string name = nameGenerator_.GetNextAvailableAssetName("NewParticle", project);

            ParticleDto newParticle = projectController_.AddParticle(name);

            addParticleToTree(newParticle);

            if (project.Actors.Count == 1)
            {
                projectController_.SelectActor(0);
            }
        }

        private TreeNode addParticleToTree(ParticleDto particle)
        {
            IParticleDtoProxy particleProxy = firemelonEditorFactory_.NewParticleProxy(projectController_, particle.Id);

            // If this particle  already has a node on the tree, ignore it.
            foreach (TreeNode node in tvParticleEmitters.Nodes["PARTICLEROOT"].Nodes)
            {
                IParticleDtoProxy nodeProxy = (IParticleDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == particleProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode particleNode = tvParticleEmitters.Nodes["PARTICLEROOT"].Nodes.Add("PARTICLE", particle.Name);

            particleNode.Tag = new AssetMenuDto(cmnuParticle, particleProxy);

            // Do these have properties? And should they?
            //particleNode.Nodes.Add("PROPERTYROOT", "Properties").Tag = new AssetMenuDto(cmnuPropertyRoot, null);

            ProjectDto project = projectController_.GetProjectDto();

            ScriptDto script = project.Scripts[particle.Id];

            IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

            particleNode.Nodes.Add("SCRIPT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);

            return particleNode;
        }

        private void addNewParticleEmitter()
        {
            ProjectDto project = projectController_.GetProjectDto();

            int particleEmitterCount = tvParticleEmitters.Nodes["PARTICLEEMITTERROOT"].Nodes.Count + 1;

            string name = nameGenerator_.GetNextAvailableAssetName("NewParticleEmitter", project);

            ParticleEmitterDto newParticleEmitter = projectController_.AddParticleEmitter(name);

            addParticleEmitterToTree(newParticleEmitter);

            if (project.Actors.Count == 1)
            {
                projectController_.SelectActor(0);
            }
        }

        private TreeNode addParticleEmitterToTree(ParticleEmitterDto particleEmitter)
        {
            IParticleEmitterDtoProxy particleEmitterProxy = firemelonEditorFactory_.NewParticleEmitterProxy(projectController_, particleEmitter.Id);

            // If this particle emitter already has a node on the tree, ignore it.
            foreach (TreeNode node in tvParticleEmitters.Nodes["PARTICLEEMITTERROOT"].Nodes)
            {
                IParticleEmitterDtoProxy nodeProxy = (IParticleEmitterDtoProxy)((AssetMenuDto)node.Tag).Asset;

                if (nodeProxy.Id == particleEmitterProxy.Id)
                {
                    node.Text = nodeProxy.Name;

                    return node;
                }
            }

            TreeNode particleEmitterNode = tvParticleEmitters.Nodes["PARTICLEEMITTERROOT"].Nodes.Add("PARTICLEEMITTER", particleEmitter.Name);

            particleEmitterNode.Tag = new AssetMenuDto(cmnuParticleEmitter, particleEmitterProxy);
            
            // Do these have properties? And should they?
            //particleEmitterNode.Nodes.Add("PROPERTYROOT", "Properties").Tag = new AssetMenuDto(cmnuPropertyRoot, null);

            ProjectDto project = projectController_.GetProjectDto();

            ScriptDto script = project.Scripts[particleEmitter.Id];

            IScriptDtoProxy scriptProxy = firemelonEditorFactory_.NewScriptProxy(projectController_, script.Id);

            particleEmitterNode.Nodes.Add("SCRIPT", "Script").Tag = new AssetMenuDto(cmnuScript, scriptProxy);

            return particleEmitterNode;
        }

        #endregion

        #region Event Handlers

        private void tvParticleEmitters_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Initialize all to default states.
            pgProperties.SelectedObject = null;
            
            switch (tvParticleEmitters.SelectedNode.Name)
            {
                case "PARTICLEEMITTER":

                    IParticleEmitterDtoProxy particleEmitterProxy = (IParticleEmitterDtoProxy)((AssetMenuDto)tvParticleEmitters.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = particleEmitterProxy;
                    
                    ((Control)pythonScriptEditorControl_).Visible = false;

                    break;

                case "PARTICLE":

                    IParticleDtoProxy particleProxy = (IParticleDtoProxy)((AssetMenuDto)tvParticleEmitters.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = particleProxy;

                    ((Control)pythonScriptEditorControl_).Visible = false;

                    break;

                //case "PROPERTY":

                //    IPropertyDtoProxy propertyProxy = (IPropertyDtoProxy)((AssetMenuDto)tvEntities.SelectedNode.Tag).Asset;

                //    pgProperties.SelectedObject = propertyProxy;

                //    break;

                case "SCRIPT":

                    IScriptDtoProxy scriptProxy = (IScriptDtoProxy)((AssetMenuDto)tvParticleEmitters.SelectedNode.Tag).Asset;

                    pgProperties.SelectedObject = scriptProxy;
                    
                    ((Control)pythonScriptEditorControl_).Visible = true;

                    pythonScriptEditorControl_.Script = scriptProxy;

                    break;

                default:
                    
                    ((Control)pythonScriptEditorControl_).Visible = false;

                    break;
            }            
        }

        private void tvParticleEmitters_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode selectedNode = tvParticleEmitters.SelectedNode;

            if (selectedNode?.Name == "SCRIPT")
            {
                DialogResult result = DialogResult.None;

                if (pythonScriptEditorControl_.ChangesMade == true)
                {
                    result = MessageBox.Show("Script contains unsaved changes. Do you want to save?", "Save Changes?", MessageBoxButtons.YesNoCancel);
                }

                if (result == DialogResult.Yes)
                {
                    pythonScriptEditorControl_.Save();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;

                    return;
                }
            }
        }

        #endregion

        private void pgProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

            switch (e.ChangedItem.Label.ToUpper())
            {
                case "(NAME)":

                    // Update the tree view with the new name.
                    string newName = e.ChangedItem.Value.ToString();

                    tvParticleEmitters.SelectedNode.Text = newName;

                    break;
            }

        }
    }
}
