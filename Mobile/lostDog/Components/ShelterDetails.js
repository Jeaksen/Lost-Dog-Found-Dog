import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity,Image } from 'react-native';
import ShelterIcon from '../Assets/animal-shelter.png'
import SkipIcon from '../Assets/skip.png';
import NormalDog from '../Assets/smallDog.png';

const {width, height} = Dimensions.get("screen")


export default class ShelterDetails extends React.Component {

  state={
    item: null
  }
  constructor(props) {
    super(props);
   }

   componentDidMount() {
    this.setState({item: this.props.item})
  }
  back=()=>
  {
    this.props.Navi.swtichPage(9,null);
  }
  showDogs=()=>
  {
    this.props.Navi.swtichPage(14,this.state.item);
  }

  render(){
    return(
      
      <View style={styles.content}>
        {
          this.state.item!=null?
            <View>
                <View style={{flexDirection: 'row', height: 100, marginHorizontal: 25}}>
                <Text style={{fontWeight: 'bold', fontSize: 30, marginVertical: 30}}>{this.state.item.name}</Text>
                </View>
                <View style={{margin: 50}}>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>Phone: </Text>{this.state.item.phoneNumber}</Text>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>Email: </Text>{this.state.item.email}</Text>
                  <Text style={{fontWeight: 'bold'}}>Address: </Text>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>City: </Text>{this.state.item.address.city}</Text>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>Street: </Text>{this.state.item.address.street}, {this.state.item.address.buildingNumber},{this.state.item.address.additionalAddressLine}</Text>
                  <Text style={{fontSize: 15}}><Text style={{fontWeight: 'bold'}}>PostCode: </Text>{this.state.item.address.postCode}</Text>
                </View>
                <TouchableOpacity style={styles.Button} onPress={() => this.showDogs()}>
                  <Image style={[styles.ButtonIcon, {marginLeft: '5%'}]} source={NormalDog} />
                  <Text style={styles.ButtonText} >ShowDogs</Text>
                </TouchableOpacity>

                <TouchableOpacity style={styles.Button} onPress={() => this.back()}>
                    <Image style={[styles.ButtonIcon, {marginLeft: '5%',transform: [{ scaleX: -1 }]}]} source={SkipIcon} />
                    <Text style={styles.ButtonText}>Back</Text>
                </TouchableOpacity>
            </View>
          :
          <View/>
        }

      </View>
    )
  }
}


const styles = StyleSheet.create({
    infoText:{
      fontSize: 15,
      margin: 2,
    },
      content: {
        margin: 30,
        alignSelf: 'center',
        justifyContent: 'center',
      },
      text:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 10,
        color: 'black',
        textAlign: 'center',
    },
    dogPic:{
      height: 100,
      width: 100,
      resizeMode: 'contain',
      aspectRatio: 1, 
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
