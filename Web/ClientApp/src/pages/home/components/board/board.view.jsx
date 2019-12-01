import React from 'react';
import { inject, observer } from 'mobx-react';
import { withRouter } from 'react-router-dom';
import { withStyles, Box, Button, Paper } from '@material-ui/core';
import { withTranslation } from 'react-i18next';
import CircularProgress from '../../../../components/progress/circular.progress';
import ColumnView from './column.view';
import CreateColumnModal from './create.column.modal';

const styles = () => ({
    root: {
        justifyContent: 'space-between',
        width: '100%'
    },
    board: {
        height: '100%',
        padding: '10px 0 10px 10px'
    },
    card: {
        display: 'flex',
        justifyContent: 'center',
        marginLeft: 10,
        height: 200,
        width: 250
    },
    column: {
        display: 'flex',
        justifyContent: 'space-between',
        flexDirection: 'column',
        alignItems: 'stretch',
        marginLeft: 10,
        height: '100%',
        width: 250
    },
    columnAddButton: {
        width: '100%',
        height: '100%'
    }
})

@inject('rootStore')
@withStyles(styles)
@withTranslation()
@withRouter
@observer
class BoardView extends React.Component {

    constructor() {
        super();

        this.fetchBoard = this.fetchBoard.bind(this);
        this.openTask = this.openTask.bind(this);
        this.openCreateColumnModal = this.openCreateColumnModal.bind(this);
        this.openCreateTaskModal = this.openCreateTaskModal.bind(this);
    }

    async componentDidMount() {
        await this.fetchBoard();
    }

    async fetchBoard(searchBy = '', assignedToMe = false, orderBy = -1) {
        const { boardId, projectId } = this.props.match.params;
        const { boardStore } = this.props.rootStore;
        await boardStore.fetchBoard(boardId, projectId, searchBy, assignedToMe, orderBy);
    }

    openTask(id) {
        const { history } = this.props;
        const { boardId, projectId } = this.props.match.params;
        history.push(`project/${projectId}/board/${boardId}/task/${id}`);
    }

    openCreateColumnModal() {
        const { modalStore } = this.props.rootStore;
        const { boardId } = this.props.match.params;

        modalStore.show(<CreateColumnModal boardId={boardId} callback={this.fetchBoard} />);
    }

    openCreateTaskModal() {
        const { modalStore } = this.props.rootStore;
        const { projectId } = this.props.match.params;

        //modalStore.show(<CreateBoardModal projectId={projectId} callback={this.fetchProject} />);
    }

    renderAddColumn() {
        const { classes, t } = this.props;
        return (
            <Paper elevation={2} className={classes.column}>
                <Button onClick={this.openCreateColumnModal} color='primary' className={classes.columnAddButton}>
                    {t('board.add column')}
                </Button>
            </Paper>
        );
    }

    render() {
        const { boardStore } = this.props.rootStore;
        const { t, classes } = this.props;

        if (boardStore.fetching)
            return <CircularProgress />;

        return (
            <Box display='flex' flexDirection='column' className={classes.root}>
                <Box display='flex' alignItems='start' flexDirection='row' flexWrap='wrap' className={classes.board}>
                    {boardStore.board.columns.map(column =>
                        <ColumnView
                            className={classes.column}
                            key={column.id}
                            id={column.id}
                            position={column.position}
                            title={column.title}
                            tasks={column.tasks}
                            canCreateTask={boardStore.board.canCreateTask}
                        />
                    )}

                    {boardStore.board.canAddColumn
                        ? this.renderAddColumn()
                        : null
                    }
                </Box>
            </Box>
        );
    }
}

export default BoardView;