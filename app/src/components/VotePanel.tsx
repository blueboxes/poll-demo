import { PollQuestion, PollResult } from '../types'
import { useState, useEffect } from 'react'
import styled from "styled-components";

const VoteButton = styled.button`
    width: 100%;
    border-radius:20px;
    margin-top:10px;
`;

const LoadingTitle = styled.h1` 
  text-align:center;
  font-size:40px;
  padding:30px;
`;

const Loading = styled.img` {
	animation: rotateY-anim 3s linear infinite;
	width: 200px;
	height: 200px;
  margin-left: auto;
  margin-right: auto;
  margin-top: 20px;
  margin-bottom: 20px;
  display: block;
}

@keyframes rotateY-anim {
	0% {
		transform: rotateY(0deg);
	}

	100% {
		transform: rotateY(360deg);
	}
}
`;
 
export default function VotePanel(props: { onVotedCallBack: ((question : PollQuestion, results: PollResult[]) => void)  }) {
 
  const [pollQuestion, setPollQuestion] = useState<PollQuestion | null>(null);
  const [isLoaded, setLoaded] = useState(false);
  const [postingVote, setPostingVote] = useState(false);

  const fetchData = async () => {
    try {
      let response = await fetch(`${import.meta.env.VITE_API_ROOT}/api/polls/3kTMd/question`);
      let json = await response.json();
      return { success: true, data: json };
    } catch (error) {
      console.log(error);
      return { success: false };
    }
  }

  const postData = async (questionId:string|undefined, answerId:number) => {
    try {
      let response = await fetch(`${import.meta.env.VITE_API_ROOT}/api/polls/3kTMd/question`, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({questionId: questionId, answerId: answerId})
      });
      let json = await response.json();
      return { success: true, data: json };
    } catch (error) {
      console.log(error);
      return { success: false };
    }
  }

  useEffect(() => {
    (async () => {
      setLoaded(false);
      let res = await fetchData();
      if (res.success) {
        setPollQuestion(res.data);
        setLoaded(true);
      }
    })();
  }, []);

  const setVote = async (id:number) => {
    setPostingVote(true);
    let res = await postData(pollQuestion!.questionId, id);
    if (res.success) {
      props.onVotedCallBack(pollQuestion!, res.data);
    }
  };

  return (
    <>
      {!isLoaded ? <><Loading src='tree.svg' /><LoadingTitle>Loading...</LoadingTitle></>  : <>
        <h1>{pollQuestion?.questionText}</h1>
        {pollQuestion?.options.map(value =>
          <VoteButton key={value.answerId} disabled={postingVote}
            onClick={() => setVote(value.answerId)}>{value.prompt}
          </VoteButton>)}
      </>}
    </>
  );
}
