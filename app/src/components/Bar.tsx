
import styled from "styled-components";

const PercentageBar = styled.div<{ value: number }>`
    width: ${props => props?.value || '0'}%;
    background-color: #4CAF50;
    height: 30px;
    border-radius:20px;
    text-align:right;
    font-size:10px;
`;

const BarContainer = styled.div`
    width: 100%;
    background-color: #ddd;
    margin: 5px;
    border-radius:20px;
`;

const BarText = styled.div`
    padding-right: 10px;
    padding-top: 5px;
    color:#fff;
`;

const BarLabel = styled.div`
    color:#fff;
    padding-top:20px;
`;
 
export default function Bar(props: { value: number, title: string}) {
    
    const displayText = props.value > 0 ?  props.value + "%" : ''

  return (
    <>
        <BarLabel>{props.title}</BarLabel>
        <BarContainer>
            <PercentageBar value={props.value}>
                <BarText>{displayText}</BarText>
            </PercentageBar>
        </BarContainer>
    </>
  );
}