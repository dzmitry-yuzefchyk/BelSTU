import React from 'react';
import { inject, observer } from 'mobx-react';
import { withTranslation } from 'react-i18next';
import { withRouter } from 'react-router-dom';
import CircularProgress from '../../../components/progress/circular.progress';
import ProjectDetails from './project.details';
import { withStyles, Box, CardActions, Card, Button } from '@material-ui/core';
import CreateProjectModal from './create.project.modal';

const styles = () => ({
    root: {
    },
    card: {
        display: 'flex',
        justifyContent: 'center',
        marginLeft: 10,
        marginRight: 10,
        marginTop: 2,
        marginBottom: 2,
        height: 150
    }
})

@inject('rootStore')
@withRouter
@withTranslation()
@withStyles(styles)
@observer
class ProjectsBoard extends React.Component {

    constructor() {
        super();

        this.openProject = this.openProject.bind(this);
        this.openCreateProjectModal = this.openCreateProjectModal.bind(this);
        this.fetchProjects = this.fetchProjects.bind(this);
    }

    async componentDidMount() {
        await this.fetchProjects();
    }

    async fetchProjects() {
        const { rootStore, page } = this.props;
        const { projectStore } = rootStore;

        await projectStore.fetchProjects(page || 0, 10);
    }

    openCreateProjectModal() {
        const { modalStore } = this.props.rootStore;
        modalStore.show(<CreateProjectModal callback={this.fetchProjects}/>);
    }

    openProject(id) {
        const { history } = this.props;
        history.push(`/project/${id}`);
    }

    render() {
        const { rootStore, t, classes } = this.props;
        const { projectStore } = rootStore;

        if (projectStore.fetching)
            return <CircularProgress />;

        return (
            <Box display='flex' alignItems='start' direction='row' className={classes.root}>
                {projectStore.projects.map(project =>
                    <ProjectDetails
                        key={project.id}
                        id={project.id}
                        title={project.title}
                        description={project.description}
                        onClick={this.openProject}
                    />
                )}
                <Card elevation={2} className={classes.card}>
                    <CardActions>
                        <Button color='primary' onClick={this.openCreateProjectModal}>
                            {t('project.create project')}
                        </Button>
                    </CardActions>
                </Card>
            </Box>
        );
    }
}

export default ProjectsBoard