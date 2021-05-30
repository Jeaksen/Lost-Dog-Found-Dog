import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import ClockIcon from '../../Assets/clock.png';
import CalendarIcon from '../../Assets/calendar.png';
import SkipIcon from '../../Assets/skip.png';

const {width, height} = Dimensions.get("screen")


export default class TimePage extends React.Component {

    
  state={
    timeRange: false,
  }

  goToNext=()=>{
    this.props.ParentRef.moveToNext();
}

    Calendar = ()=>{
        this.setState({timeRange: true})
      }

  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Step 3/7 - Time</Text>
            {
              !this.state.timeRange?
            <View>
                <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                    <Image style={[styles.ButtonIcon,{marginLeft: '5%'}]} source={ClockIcon} />
                    <Text style={styles.ButtonText} >I see him now</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.Button} onPress={() => this.Calendar()}>
                    <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={CalendarIcon} />
                    <Text style={styles.ButtonText} >I saw him before</Text>
                </TouchableOpacity>
            </View>:

            <View style={{marginTop: 10}}>
                <TextInput style={styles.inputtext} placeholder="From ..."/>
                <TextInput style={styles.inputtext} placeholder="To ..."/>
                <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                        <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={SkipIcon} />
                        <Text style={styles.ButtonText}>Continue</Text>
                </TouchableOpacity>
            </View>
            }
        </View>
  )
  }
}


const styles = StyleSheet.create({
    content: {
        height: '100%',
        margin: 50,
        alignSelf: 'center',
        marginVertical: 'auto',
    },
    Title:{
        fontSize: 20,
        marginTop: 10,
        alignSelf: 'center',
        color: '#99481f',
    },
    Button:{
        backgroundColor: '#feb26d',
        width: width*0.5,
        height: height*0.06,
        margin: 20,
        marginLeft: 'auto',
        marginRight: 'auto',
        flexDirection: 'row',
        alignContent: 'center',
        borderRadius: 15,
    },
    ButtonText:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 15,
        color: 'white',
        textAlign: 'center',
        width: '75%',
    },
    ButtonIcon:{
        width: 35,
        height:35,
        alignSelf: 'center',
    },
    inputtext: {
        fontSize: 16,
        height: 30,
        width: width*0.5,
        borderColor: '#000000',
        borderWidth: 1,
        borderRadius: 5,
        paddingLeft: 5,
        marginVertical: 10,
      },
});
