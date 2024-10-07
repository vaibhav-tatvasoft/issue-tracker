export const SET_ISSUES = 'SET_ISSUES';
export const setIssues = (issues) => {
    return {
      type: SET_ISSUES,
      payload: issues,
    };
  };