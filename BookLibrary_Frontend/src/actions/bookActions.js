import axios from "axios";
import {
  BOOK_LIST_REQUEST,
  BOOK_LIST_SUCCESS,
  BOOK_LIST_FAIL,
} from "../constants/bookConstants";

export const listBooks =
  (page = 1) =>
  async (dispatch) => {
    try {
      dispatch({ type: BOOK_LIST_REQUEST });
      const { data } = await axios.get(
        `http://localhost:51202/api/books?page=${page}&item=10`
      );
      dispatch({ type: BOOK_LIST_SUCCESS, payload: data });
    } catch (error) {
      dispatch({
        type: BOOK_LIST_FAIL,
        payload:
          error.response && error.response.data.datail
            ? error.response.data.detail
            : error.Message,
      });
    }
  };
