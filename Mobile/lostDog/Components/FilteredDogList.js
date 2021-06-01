import * as React from 'react';
import { View, StyleSheet,Text,TextInput,Dimensions,TouchableOpacity ,ScrollView,SafeAreaView,FlatList,Image} from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import DogListItem from './Helpers/DogListItem';
import found from '../Assets/found.png'

const {width, height} = Dimensions.get("screen")



export default class FilteredDogList extends React.Component {
    state={
        info: "normal",
        image: null,
    
        DogList: [],

      }

      getDogList = ()=>{
        console.log("---------------- FILTERED LOS DOG LIST: ---------------------------------")
        //console.log(this.props.item);
        this.props.Navi.RunOnBackend("getFilteredDogList",this.props.item).then((responseData)=>{
          //console.log(responseData)
          this.setState({DogList: responseData});
          if(responseData.length==0)
          {
            this.setState({info: "noDogs"})
          }
          else{
            this.setState({info: "success"})
          }
          console.log("succes list of dogs is ready")
        }).catch((x)=>
            {
            console.log("Login Error" + (x))
            this.setState({info: "error"})
            }
          )
      }
    
      loadingDogListFailed=()=>{
    
      }
    
      pickImage = async () => {
        let result = await ImagePicker.launchImageLibraryAsync({
          mediaTypes: ImagePicker.MediaTypeOptions.All,
          allowsEditing: false,
          aspect: [4, 3],
          quality: 1,
        });
    
        if (!result.cancelled) {
          //console.log(result);
          this.setState({image: result.uri});
        }
      };
    
      getDogInfo = ()=>{
        console.log("getDogInfo Button");
      }
    
      constructor(props) {
        super(props);
        this.getDogList();
       }
    
      dogSelected=(item)=>{
        this.props.Navi.swtichPage(11,{data:item,filterInfo:this.props.item});
      }
      render(){
        return(
            <View style={styles.content}>
                <View style={[{flexDirection: 'row', width: 0.8*width, margin: 0}]}>
                    <Image source={found} style={[styles.Icon,{width: 100, height:100}]}/>
                    <Text  style={[{ width: '70%', fontSize: 25, fontWeight: 'bold', textAlignVertical: 'center', color: '#99481f'}]}>Which of these dogs matches the one you saw?</Text>
                </View>
                {
                  this.state.info=="normal"?
                  <Text style={{alignSelf: 'center', marginVertical: 0.2*height, width: 0.5*width, fontSize: 20}}>
                    We are looking for dogs that match what you saw ... 
                  </Text>:
                  this.state.info=="error"?
                  <Text style={{alignSelf: 'center', marginVertical: 0.2*height, width: 0.5*width, fontSize: 20}}>
                    There was an error while connecting to the server.
                  </Text>:
                  this.state.info=="noDogs"?
                  <Text style={{alignSelf: 'center', marginVertical: 0.2*height, width: 0.5*width, fontSize: 20}}>
                    Unfortunately, no one has reported such a dog missing, now you can only contact the shelter.
                  </Text>:
                  this.state.info=="success"?
                  <FlatList
                      data={this.state.DogList.length>0 ? this.state.DogList : []}
                      renderItem={({item}) => <DogListItem item={item} dogSelected={this.dogSelected}/>}
                      keyExtractor={(item) => item.id.toString()}
                      />:
                  <View/>
                }
            </View>
      )
      }
    }
    
    const styles = StyleSheet.create({
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
      content: {
        marginTop:30,
        margin: 15,
        height: '90%',
        alignSelf: 'center',
        justifyContent: 'center',
      },
      loginButton:{
        marginTop: 20,
        backgroundColor: 'black',
        width: width*0.2,
        height: height*0.05,
        marginLeft: 'auto',
        marginRight: 'auto',
    },
    logintext:{
        marginTop: 'auto',
        marginBottom: 'auto',
        fontSize: 15,
        color: 'white',
        textAlign: 'center',
    },
    Title:{
      marginBottom: 50,
      fontSize: 20,
      textAlign: 'center',
      fontWeight: 'bold',
    },
    });