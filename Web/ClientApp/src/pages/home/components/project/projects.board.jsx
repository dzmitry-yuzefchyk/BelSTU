import React from 'react';
import { inject, observer } from 'mobx-react';
import { withTranslation } from 'react-i18next';
import { withRouter } from 'react-router-dom';
import CircularProgress from '../../../../components/progress/circular.progress';
import ProjectDetails from './project.details';
import { withStyles, Box, CardActions, Card, Button } from '@material-ui/core';
import CreateProjectModal from './create.project.modal';
import Pagination from '@kevinwang0316/react-materialui-pagination';

const styles = () => ({
    root: {
    },
    card: {
        display: 'flex',
        justifyContent: 'center',
        marginLeft: 10,
        marginRight: 10,
        marginTop: 10,
        marginBottom: 10,
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
        this.openPage = this.openPage.bind(this);
    }

    async componentDidMount() {
        await this.fetchProjects();
    }

    async componentDidUpdate(prevProps) {
        if (prevProps.page !== this.props.page)
            await this.fetchProjects();
    }

    async fetchProjects() {
        const { rootStore, page } = this.props;
        const { projectStore } = rootStore;

        await projectStore.fetchProjects(page || 0, 10);
    }

    openPage(page) {
        const { history } = this.props;
        history.push(`/projects?page=${page}`);
    }

    openCreateProjectModal() {
        const { modalStore } = this.props.rootStore;
        modalStore.show(<CreateProjectModal callback={this.fetchProjects} />);
    }

    openProject(id) {
        const { history } = this.props;
        history.push(`/project/${id}`);
    }

    render() {
        const { rootStore, t, classes, page } = this.props;
        const { projectStore } = rootStore;

        if (projectStore.fetching)
            return <CircularProgress />;

        return (
            <React.Fragment>
                <Box display='flex' alignItems='start' direction='row' flexWrap='wrap' className={classes.root}>
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

                <Box display='flex' justifyContent='center'>
                    <Pagination
                        offset={Number(page)}
                        limit={1}
                        total={projectStore.total || 0}
                        onClick={this.openPage}
                    />
                </Box>
            </React.Fragment>
        );
    }
}

export default ProjectsBoard