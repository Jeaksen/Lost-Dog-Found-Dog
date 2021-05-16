import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Animated,Image } from 'react-native';
import RunFrame1 from '../../Assets/RunAnim/runAnim_1.png'
import RunFrame2 from '../../Assets/RunAnim/runAnim_2.png'
import RunFrame3 from '../../Assets/RunAnim/runAnim_3.png'
import RunFrame4 from '../../Assets/RunAnim/runAnim_4.png'

const {width, height} = Dimensions.get("screen")
const speed=100
const delta=1

export default class LoadingPage extends React.Component {
  state={
    CurrentFrame: RunFrame1,
  }

  componentDidMount(){
   // this.idle()
   //this.runAnim()
  }
  runAnim () 
  {
   setTimeout(()=>{this.setState({ CurrentFrame: RunFrame1 })},1 * speed);
   setTimeout(()=>{this.setState({ CurrentFrame: RunFrame2 })},3 * speed);
   setTimeout(()=>{this.setState({ CurrentFrame: RunFrame3 })},5 * speed);
   setTimeout(()=>{this.setState({ CurrentFrame: RunFrame4 })},7 * speed);
   setTimeout(()=>{this.setState({ CurrentFrame: RunFrame3 })},9 * speed);
   setTimeout(()=>{this.setState({ CurrentFrame: RunFrame2 })},10 * speed);
   setTimeout(()=>{this.setState({ CurrentFrame: RunFrame1 })},12 * speed);

  }


  render(){
    return(
        <View style={styles.content}>
            <Text style={styles.logintext} />
            <Image source={this.state.CurrentFrame} style={styles.Icon}/>
        </View>
  )
  }
}


const styles = StyleSheet.create({
  WhiteBack:{
    width: 0.8*width,
    height: 0.3*height,
    backgroundColor: 'red',
    marginTop: 'auto',
    marginBottom: 'auto',
  },
  Icon:{
    resizeMode: 'contain',
    aspectRatio: 0.25, 
    alignSelf: 'center',
    marginTop: 'auto',
    marginBottom: 'auto',
    },
  content: {
    position: 'absolute',
    backgroundColor: 'white',
    height: '100%',
    width: '100%',
    borderRadius: 100,
    marginHorizontal: 30,
    alignSelf: 'center',
    justifyContent: 'center',
    marginVertical: 'auto',
  },
    logintext:{
    marginTop: 'auto',
    marginBottom: 'auto',
    fontSize: 25,
    textAlign: 'center',
    backgroundColor: 'white',
    height: 0.5*height,
    borderRadius: 100,
    textAlignVertical: 'center'
}
});
