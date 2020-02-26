import React from 'react';
import { inject, observer } from 'mobx-react';
import { withRouter } from 'react-router-dom';
import CircularProgress from '../../../../components/progress/circular.progress';
import { withStyles, Box, CardActions, Card, Button, Tabs, Tab } from '@material-ui/core';
import BoardDetails from './../board/board.details';
import { withTranslation } from 'react-i18next';
import CreateBoardModal from '../board/create.board.modal';
import ProjectSettingsForm from './project.settings.form';
import ProjectAccessForm from './project.access.form';

const tabs = {
    boards: 0,
    settings: 1,
    access: 2
};


const styles = theme => ({
    root: {
        overflowY: 'auto',
        width: '100%'
    },
    content: {
        justifyContent: 'space-between'
    },
    board: {
        padding: 10,
        flex: '2 2 auto'
    },
    card: {
        display: 'flex',
        justifyContent: 'center',
        marginBottom: 10,
        height: 150,
        width: '100%'
    },
    boardHeader: {
        width: '100%',
        backgroundColor: theme.palette.primary.main
    }
})

const ProjectTab = props => {
    const { children, value, index } = props;

    return (
        <Box hidden={value !== index}>
            {children}
        </Box>
    );
};

@inject('rootStore')
@withStyles(styles)
@withTranslation()
@withRouter
@observer
class ProjectView extends React.Component {

    constructor() {
        super();

        this.state = {
            currentTab: 0,
            accessPage: 0
        };

        this.openBoard = this.openBoard.bind(this);
        this.openCreateBoardModal = this.openCreateBoardModal.bind(this);
        this.fetchProject = this.fetchProject.bind(this);
        this.handleTabChange = this.handleTabChange.bind(this);
        this.fetchSettings = this.fetchSettings.bind(this);
        this.fetchAccess = this.fetchAccess.bind(this);
    }

    async componentDidMount() {
        await this.fetchProject();
    }

    async fetchProject() {
        const { projectId } = this.props.match.params;
        const { projectStore } = this.props.rootStore;
        await projectStore.fetchProject(projectId);
    }

    async fetchSettings() {
        const { projectId } = this.props.match.params;
        const { projectStore } = this.props.rootStore;
        await projectStore.fetchSettings(projectId);
    };

    async fetchAccess(page = 0) {
        const { projectId } = this.props.match.params;
        const { projectStore } = this.props.rootStore;
        await projectStore.fetchAccess(projectId, page);
    };

    openBoard(id) {
        const { history } = this.props;
        const { projectId } = this.props.match.params;
        history.push(`/project/${projectId}/board/${id}`);
    }

    openCreateBoardModal() {
        const { modalStore } = this.props.rootStore;
        const { projectId } = this.props.match.params;

        modalStore.show(<CreateBoardModal projectId={projectId} callback={this.fetchProject} />);
    }

    async handleTabChange(event, tab) {
        this.setState({
            currentTab: tab
        });

        if (tab === tabs.boards) await this.fetchProject();
        else if (tab === tabs.settings) await this.fetchSettings();
        else if (tab === tabs.access) await this.fetchAccess();
    }

    renderBoards() {
        const { projectStore } = this.props.rootStore;
        const { t, classes } = this.props;

        return (
            <Box display='flex' alignItems='start' flexDirection='column' flexWrap='wrap' className={classes.board}>
                {projectStore.project.boards.map(board =>
                    <BoardDetails
                        key={board.id}
                        id={board.id}
                        title={board.title}
                        onClick={this.openBoard}
                    />
                )}
                {projectStore.project.canAddBoard && projectStore.project.canCreateBoard
                    ? <Card elevation={2} className={classes.card}>
                        <CardActions>
                            <Button color='primary' onClick={this.openCreateBoardModal}>
                                {t('project.create board')}
                            </Button>
                        </CardActions>
                    </Card>
                    : null
                }
            </Box>
        );
    }

    render() {
        const { projectStore } = this.props.rootStore;
        const { t, classes } = this.props;
        const { currentTab } = this.state;
        const { projectId } = this.props.match.params;

        return (
            <Box display='flex' flexDirection='column' className={classes.root}>
                <Box className={classes.boardHeader}>
                    <Tabs value={currentTab} onChange={this.handleTabChange} >
                        <Tab label={t('project.boards')} />
                        <Tab disabled={!projectStore.project.canUpdateProject} label={t('project.settings')} />
                        <Tab disabled={!projectStore.project.canChangeSecurity} label={t('project.access')} />
                        <Tab disabled label={projectStore.project.title}></Tab>
                    </Tabs>
                </Box>
                {projectStore.fetching
                    ? <CircularProgress />
                    : <Box display='flex' flexDirection='column' className={classes.content}>
                        <ProjectTab value={currentTab} index={tabs.boards}>
                            {this.renderBoards()}
                        </ProjectTab>
                        <ProjectTab value={currentTab} index={tabs.settings}>
                            <ProjectSettingsForm projectId={projectId} />
                        </ProjectTab>
                        <ProjectTab value={currentTab} index={tabs.access}>
                            <ProjectAccessForm projectId={projectId} fetchAccess={this.fetchAccess} />
                        </ProjectTab>
                    </Box>
                }

            </Box>
        );
    }
}


export default ProjectView;