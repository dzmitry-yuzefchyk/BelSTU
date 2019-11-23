import React from 'react';
import { inject, observer } from 'mobx-react';
import { withTranslation } from 'react-i18next';
import { withRouter } from 'react-router-dom';
import CircularProgress from '../../../components/progress/circular.progress';
import ProjectDetails from './project.details';
import { withStyles, Box, Typography } from '@material-ui/core';

const styles = () => ({
    root: {
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
    }

    async componentDidMount() {
        const { rootStore } = this.props;
        const { projectStore } = rootStore;
        
        await projectStore.fetchProjects(0, 10);
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

        if (projectStore.isEmpty)
            return <Typography>{t('project.there is no projects yet')}</Typography>

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
            </Box>
        );
    }
}

export default ProjectsBoard