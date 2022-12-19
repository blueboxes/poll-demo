import Bar from './Bar'
import { PollQuestion, PollResult } from '../types'

import styled from "styled-components";

const Title = styled.h1`
    padding-bottom: 40px;
`;

export default function Results(props: { question: PollQuestion | undefined, results : PollResult[] | [] }) {

  return (
    <>
      <Title>{props?.question?.questionText}</Title>
      {props?.question?.options.map(value => {
        const percentage = props?.results.find(a => a.answerId == value.answerId)?.percentage ?? 0;
        return <Bar key={value.answerId} value={percentage} title={value.prompt} />
      })}
    </>
  );
}
