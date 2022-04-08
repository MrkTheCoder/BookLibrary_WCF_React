import axios from "axios";
import {
  BORROWER_LIST_FAIL,
  BORROWER_LIST_REQUEST,
  BORROWER_LIST_SUCCESS,
} from "../constants/borrowersConstants";

export const listBorrowers =
  (page = 1, item = 10) =>
  async (dispatch) => {
    try {
      dispatch({ type: BORROWER_LIST_REQUEST });
      const { data, headers } = await axios.get(
        `http://localhost:51204/api/borrowers?page=${page}&item=${item}`
      );
      dispatch({
        type: BORROWER_LIST_SUCCESS,
        payload: data,
        headers: headers,
      });
    } catch (error) {
      dispatch({
        type: BORROWER_LIST_FAIL,
        payload:
          error.response && error.response.data.Message
            ? error.response.data.Message
            : error.message,
      });
    }
  };
