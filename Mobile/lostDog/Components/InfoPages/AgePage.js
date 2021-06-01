import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import youngDog from '../../Assets/youngDog.png';
import NormalDog from '../../Assets/NormalDog.png';
import OldDog from '../../Assets/OldDog.png';
import SkipIcon from '../../Assets/skip.png';

const {width, height} = Dimensions.get("screen")


export default class LocationPage extends React.Component {

    save=(f,t)=>{
        this.props.ParentRef.setAge(
            {
                ageFrom: f,
                ageTo: t,
            }
        )
        this.goToNext()
    }
    goToNext=()=>{
        this.props.ParentRef.moveToNext();
    }

  render(){
    return(
        <View style={styles.content}>
          <Text style={styles.Title}>Step 5/7 - Age</Text>
          <View>
            <TouchableOpacity style={styles.Button} onPress={() => this.save(0,2)}>
                <Image style={[styles.ButtonIcon,{marginLeft: '5%'}]} source={youngDog} />
                <Text style={styles.ButtonText} >Young 0-2</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.save(2,9)}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={NormalDog} />
                <Text style={styles.ButtonText} >Average 2-9</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.save(9,100)}>
                <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={OldDog} />
                <Text style={styles.ButtonText} >Old 9+</Text>
            </TouchableOpacity>

            <TouchableOpacity style={styles.Button} onPress={() => this.goToNext()}>
                    <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={SkipIcon} />
                    <Text style={styles.ButtonText} >Skip</Text>
            </TouchableOpacity>

          </View>
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
    }
});
