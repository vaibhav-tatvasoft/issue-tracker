import { combineReducers } from 'redux';
import IssueReducer from './IssueReducer';

const rootReducer = combineReducers({
    issues: IssueReducer});
export default rootReducer;