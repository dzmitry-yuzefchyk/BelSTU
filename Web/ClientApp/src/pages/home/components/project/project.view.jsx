import React from 'react';
import { inject, observer } from 'mobx-react';
import { withRouter } from 'react-router-dom';
import CircularProgress from '../../../../components/progress/circular.progress';
import { withStyles, Box, CardActions, Card, Button } from '@material-ui/core';
import BoardDetails from './../board/board.details';
import { withTranslation } from 'react-i18next';
import CreateBoardModal from '../board/create.board.modal';

const styles = () => ({
    root: {
        justifyContent: 'space-between',
        width: '100%'
    },
    board: {
        overflowY: 'auto'
    },
    card: {
        display: 'flex',
        justifyContent: 'center',
        marginLeft: 10,
        height: 200,
        width: 250
    }
})

@inject('rootStore')
@withStyles(styles)
@withTranslation()
@withRouter
@observer
class ProjectView extends React.Component {

    constructor() {
        super();

        this.openBoard = this.openBoard.bind(this);
        this.openCreateBoardModal = this.openCreateBoardModal.bind(this);
        this.fetchProject = this.fetchProject.bind(this);
    }

    async componentDidMount() {
        await this.fetchProject();
    }

    async fetchProject() {
        const { projectId } = this.props.match.params;
        const { projectStore } = this.props.rootStore;
        await projectStore.fetchProject(projectId);
    }

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

    render() {
        const { projectStore } = this.props.rootStore;
        const { t, classes } = this.props;

        if (projectStore.fetching)
            return <CircularProgress />;

        return (
            <Box display='flex' flexDirection='column' className={classes.root}>
                <Box display='flex' alignItems='start' flexDirection='row' flexWrap='wrap' className={classes.board}>
                    {projectStore.project.boards.map(board =>
                        <BoardDetails
                            key={board.id}
                            id={board.id}
                            title={board.title}
                            onClick={this.openBoard}
                        />
                    )}
                    {projectStore.project.canAddBoard
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
            </Box>
        );
    }
}

export default ProjectView;